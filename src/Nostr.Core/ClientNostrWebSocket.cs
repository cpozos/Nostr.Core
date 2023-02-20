using System.Net.WebSockets;

namespace Nostr.Core;

public class ClientNostrWebSocket : IDisposable
{
    private ClientWebSocket? _clientWebSocket;
    private bool disposedValue;

    public ClientNostrWebSocket()
    {
        _clientWebSocket = new ClientWebSocket()
        {
            Options =
            {
                KeepAliveInterval = TimeSpan.FromSeconds(15)
            }
        };
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                _clientWebSocket?.Dispose();
            }

            _clientWebSocket = null;
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