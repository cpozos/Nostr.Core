using Nostr.Core.Models;
using Nostr.Core.Interfaces;
using System.Threading.Channels;

namespace Nostr.Core;

public class NostrRelay : INostrResponsesDispatcher
{
    private readonly Channel<NostrMessage> _channel = Channel.CreateUnbounded<NostrMessage>();

    public async Task StartReadMessages(CancellationToken cancellationToken)
    {
        //while (await _channel.Reader.WaitToReadAsync())
        //{
        //    if (!_channel.Reader.TryRead(out NostrMessage? message))
        //    {
        //        continue;
        //    }

        //    try
        //    {
        //        if (!_connections.TryGetValue(message.ConnectionId, out INostrConnection? conn))
        //        {
        //            continue;
        //        }

        //        await conn.SendMessage(message, cancellationToken);
        //    }
        //    catch when (cancellationToken.IsCancellationRequested)
        //    {
        //        throw;
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //}
    }
}