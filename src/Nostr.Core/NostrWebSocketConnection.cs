using Nostr.Core.DTOs;
using System.Net.WebSockets;
using System.Text;

namespace Nostr.Core;
public class NostrWebSocketConnection : INostrConnection
{
    private readonly WebSocket _socket;

    public NostrWebSocketConnection(string id, WebSocket socket)
	{
        _socket = socket;
        Id = id;
	}

    public string Id { get; set; }

    public async Task SendMessage(NostrMessage nostrMessage, CancellationToken cancellationToken)
    {
        if (_socket.State != WebSocketState.Open)
            return;

        var buffer = Encoding.UTF8.GetBytes(nostrMessage.Message);
        await _socket.SendAsync(new Memory<byte>(buffer), WebSocketMessageType.Text, true, cancellationToken);
    }
}