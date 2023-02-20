using Nostr.Core;
using Nostr.Core.Communication;
using Nostr.Core.DTOs;
using Nostr.Core.Interfaces;
using System.Collections.Concurrent;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Text;

internal class NostrMiddleware : IMiddleware
{
    private static readonly INostrRelay _relay = new NostrRelay();
    private readonly NostrMessageHandler _nostrMessageHandler;
    private readonly ConcurrentDictionary<string, WebSocket> _webScokets = new();

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
        using WebSocket socket = await context.WebSockets.AcceptWebSocketAsync();
       
        string connectionId = context.TraceIdentifier;
        _webScokets.TryAdd(connectionId, socket);

        while (socket.State == WebSocketState.Open)
        {
            var arraySegment = new ArraySegment<byte>(new byte[1024 * 4]);
            string message;
            WebSocketReceiveResult result;

            try
            {
                using var ms = new MemoryStream();
                do
                {
                    result = await socket.ReceiveAsync(arraySegment, CancellationToken.None)
                        .ConfigureAwait(false);

                    ms.Write(arraySegment.Array!, arraySegment.Offset, result.Count);
                } while (!result.EndOfMessage);

                ms.Seek(0, SeekOrigin.Begin);
                using var reader = new StreamReader(ms, Encoding.UTF8);
                message = await reader.ReadToEndAsync().ConfigureAwait(false);
            }
            catch (Exception e)
            {
                throw e;
            }

            if (result.MessageType == WebSocketMessageType.Text)
            {
                await _nostrMessageHandler.HandleMessage(new NostrMessage(connectionId, message));
            }
            else if (result.MessageType == WebSocketMessageType.Close)
            {
                await _nostrMessageHandler.Disconnect(new NostrMessage(connectionId, message));
            }
        }

        _webScokets.TryRemove(context.TraceIdentifier, out var _);
    }
}