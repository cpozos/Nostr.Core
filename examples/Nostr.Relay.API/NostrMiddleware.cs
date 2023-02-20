using Nostr.Core;
using Nostr.Core.Models;
using Nostr.Core.Interfaces;
using System.Net.WebSockets;

internal class NostrMiddleware : IMiddleware
{
    private static readonly INostrRelay _relay = new NostrRelay();
    private readonly NostrMessageHandler _nostrMessageHandler;

    public NostrMiddleware(NostrMessageHandler nostrEventHandler)
    {
        _nostrMessageHandler = nostrEventHandler;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        if (!context.WebSockets.IsWebSocketRequest)
        {
            await next.Invoke(context);
            return;
        }

        await DoReceiveMessages(context);
    }

    private async Task DoReceiveMessages(HttpContext context)
    {
        using NostrWebSocketConnection socket = new NostrWebSocketConnection(context.TraceIdentifier, await context.WebSockets.AcceptWebSocketAsync());
        _relay.AddConnection(socket);

        while (socket.IsConnectionOpen)
        {
            var response = await socket.ReceiveMessage();

            if (response.Result.MessageType == WebSocketMessageType.Text)
            {
                await _nostrMessageHandler.HandleMessage(new NostrMessage(socket.Id, response.Message));
            }
            else if (response.Result.MessageType == WebSocketMessageType.Close)
            {
                await _nostrMessageHandler.Disconnect(new NostrMessage(socket.Id, response.Message));
            }
        }

        _relay.RemoveConnection(socket);
    }
}