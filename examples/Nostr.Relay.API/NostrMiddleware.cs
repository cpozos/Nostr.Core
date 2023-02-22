using Nostr.Core;
using Nostr.Core.Models;
using Nostr.Core.Interfaces;
using System.Net.WebSockets;

internal class NostrMiddleware : IMiddleware
{
    private static readonly INostrRelay _relay = new NostrRelay();
    private readonly NostrMessagePropagator _messagePropagator;

    public NostrMiddleware(NostrMessagePropagator messagePropagator)
    {
        _messagePropagator = messagePropagator;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        if (!context.WebSockets.IsWebSocketRequest)
        {
            await next.Invoke(context);
            return;
        }

        using NostrWebSocketConnection socket = new (context.TraceIdentifier, await context.WebSockets.AcceptWebSocketAsync());
        _relay.AddConnection(socket);

        while (socket.IsConnectionOpen)
        {
            var (Result, Message) = await socket.ReceiveMessage();

            if (Result.MessageType == WebSocketMessageType.Text)
            {
                await _messagePropagator.HandleMessage(new NostrMessage(socket.Id, Message), socket);
            }
            else if (Result.MessageType == WebSocketMessageType.Close)
            {
                await _messagePropagator.Disconnect(new NostrMessage(socket.Id, Message), socket);
            }
        }

        _relay.RemoveConnection(socket);
    }
}