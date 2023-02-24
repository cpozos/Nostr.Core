using Nostr.Core;
using Nostr.Core.Models;
using Nostr.Core.Interfaces;
using System.Net.WebSockets;
using Nostr.Core.Persistence;

internal class NostrMiddleware : IMiddleware
{
    private readonly INostrRelay _relay;
    private readonly INostrRepo _nostrRepo;
    private readonly NostrMessagePropagator _messagePropagator;

    public NostrMiddleware(INostrRelay relay, NostrMessagePropagator messagePropagator, INostrRepo nostrRepo)
    {
        _messagePropagator = messagePropagator;
        _relay = relay;
        _nostrRepo = nostrRepo;
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
        _nostrRepo.AddConnection(socket);

        while (socket.IsConnectionOpen)
        {
            var (Result, Message) = await socket.ReceiveMessage();

            if (Result.MessageType == WebSocketMessageType.Text)
            {
                await _messagePropagator.HandleMessage(new NostrMessage(socket.Id, Message), socket, _nostrRepo);
            }
            else if (Result.MessageType == WebSocketMessageType.Close)
            {
                await _messagePropagator.Disconnect(new NostrMessage(socket.Id, Message), socket, _nostrRepo);
            }
        }

        _nostrRepo.RemoveConnection(socket);
        _relay.RemoveConnection(socket);
    }
}