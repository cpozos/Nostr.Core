using Nostr.Core.Models;
using Nostr.Core.Interfaces;
using System.Collections.Concurrent;
using System.Threading.Channels;

namespace Nostr.Core;

public class NostrRelay : INostrRelay, INostrResponsesDispatcher
{
    private readonly Channel<NostrMessage> _channel = Channel.CreateUnbounded<NostrMessage>();

    private readonly ConcurrentDictionary<string, INostrConnection> _connections = new();

	public void AddConnection(INostrConnection connection)
	{
		_connections.TryAdd(connection.Id, connection);
	}

    public INostrConnection[] GetConnections(params string[] ids)
    {
        return _connections.Where(x => ids.Contains(x.Key)).Select(x => x.Value).ToArray();
    }

    public void RemoveConnection(INostrConnection connection)
    {
        _connections.Remove(connection.Id, out var _);
    }

    public async Task StartReadMessages(CancellationToken cancellationToken)
    {
        while (await _channel.Reader.WaitToReadAsync())
        {
            if (!_channel.Reader.TryRead(out NostrMessage? message))
            {
                continue;
            }

            try
            {
                if (!_connections.TryGetValue(message.ConnectionId, out INostrConnection? conn))
                {
                    continue;
                }

                await conn.SendMessage(message, cancellationToken);
            }
            catch when (cancellationToken.IsCancellationRequested)
            {
                throw;
            }
            catch (Exception ex)
            {
            }
        }
    }
}