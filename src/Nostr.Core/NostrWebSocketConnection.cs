using Nostr.Core.Models;
using System.Net.WebSockets;
using System.Text;

namespace Nostr.Core;

public class NostrWebSocketConnection : INostrConnection
{
    private readonly WebSocket _socket;
    private bool disposedValue;

    public NostrWebSocketConnection(string id, WebSocket socket)
	{
        _socket = socket;
        Id = id;
	}

    public string Id { get; set; }

    public bool IsConnectionOpen => _socket?.State == WebSocketState.Open;

    public async Task SendMessage(NostrMessage nostrMessage, CancellationToken cancellationToken)
    {
        if (_socket is null || _socket.State != WebSocketState.Open)
            return;

        var buffer = Encoding.UTF8.GetBytes(nostrMessage.Message);
        await _socket.SendAsync(new Memory<byte>(buffer), WebSocketMessageType.Text, true, cancellationToken);
    }

    public async Task<(WebSocketReceiveResult Result, string Message)> ReceiveMessage()
    {
        var arraySegment = new ArraySegment<byte>(new byte[1024 * 4]);
        string message;
        WebSocketReceiveResult result;

        try
        {
            using var ms = new MemoryStream();
            do
            {
                result = await _socket.ReceiveAsync(arraySegment, CancellationToken.None)
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

        return (result, message);
    }

    public async Task Disconnect()
    {
        if (_socket.State == WebSocketState.Open)
        {
            await _socket.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None);
        }
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                _socket?.Dispose();
            }

            disposedValue = true;
        }
    }

    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}