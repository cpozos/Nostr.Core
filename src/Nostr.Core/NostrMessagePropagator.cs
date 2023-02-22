using Nostr.Core.Models;
using Nostr.Core.EventHandlers;
using Nostr.Core.Interfaces;
using Nostr.Core.Persistence;

namespace Nostr.Core;

public class NostrMessagePropagator
{
    private readonly INostrRequestEventHandler nostrRequestHandler = new NostrRequestEventHandler();
    private readonly INostrPublishEventHandler nostrPublishHandler = new NostrPublishEventHandler();
    private readonly INostrCloseEventHandler nostrCloseHandler = new NostrCloseEventHandler();
    private readonly INostrRepo nostrEventRepo = new NostrRepo();

    public Task HandleMessage(NostrMessage nostrMessage, INostrConnection connection)
    {
        if (nostrMessage.Message.StartsWith("[\"REQ", StringComparison.OrdinalIgnoreCase))
        {
            return nostrRequestHandler.Handle(nostrMessage, connection, nostrEventRepo);
        }
        else if (nostrMessage.Message.StartsWith("[\"EVENT", StringComparison.OrdinalIgnoreCase))
        {
            return nostrPublishHandler.Handle(nostrMessage, nostrEventRepo);
        }
        else if (nostrMessage.Message.StartsWith("[\"CLOSE", StringComparison.OrdinalIgnoreCase))
        {
            return Disconnect(nostrMessage, connection);
        }
        else
        {
            return Task.CompletedTask;
        }
    }

    public async Task Disconnect(NostrMessage nostrMessage, INostrConnection connection)
    {
        await nostrCloseHandler.Handle(nostrMessage, nostrEventRepo);

        // TOOD: Should it be disconnected ?
        await connection.Disconnect();
    }
}