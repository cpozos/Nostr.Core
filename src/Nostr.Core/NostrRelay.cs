using Nostr.Core.Models;
using Nostr.Core.Interfaces;
using System.Collections.Concurrent;
using System.Threading.Channels;

namespace Nostr.Core;

public class NostrRelay : INostrRelay
{
    public readonly Channel<NostrMessage> PendingMessages = Channel.CreateUnbounded<NostrMessage>();

    private readonly ConcurrentDictionary<string, INostrConnection> _connections = new();

	public void AddConnection(INostrConnection connection)
	{
		_connections.TryAdd(connection.Id, connection);
	}

    public void RemoveConnection(INostrConnection connection)
    {
        _connections.Remove(connection.Id, out var _);
    }

    public async Task ProcessSendMessages(CancellationToken cancellationToken)
    {
        while (await PendingMessages.Reader.WaitToReadAsync(cancellationToken))
        {
            try
            {
                if (PendingMessages.Reader.TryRead(out var message))
                {
                    if (_connections.TryGetValue(message.ConnectionId, out var conn))
                    {
                        await conn.SendMessage(message, cancellationToken);
                    }
                }
            }
            catch when (cancellationToken.IsCancellationRequested)
            {
                throw;
            }
            catch (Exception e)
            {
            }
        }
    }
}