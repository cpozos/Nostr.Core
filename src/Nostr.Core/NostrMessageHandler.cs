using Nostr.Core.Models;
using Nostr.Core.EventHandlers;
using Nostr.Core.Interfaces;

namespace Nostr.Core;

public class NostrMessageHandler
{
    private readonly INostrRequestEventHandler nostrRequestHandler = new NostrRequestEventHandler();
    private readonly INostrPublishEventHandler nostrPublishHandler = new NostrPublishEventHandler();
    private readonly INostrCloseEventHandler nostrCloseHandler = new NostrCloseEventHandler();

    public Task HandleMessage(NostrMessage nostrMessage)
    {
        if (nostrMessage.Message.StartsWith("[\"REQ", StringComparison.OrdinalIgnoreCase))
        {
            return nostrRequestHandler.Handle(nostrMessage);
        }
        else if (nostrMessage.Message.StartsWith("[\"EVENT", StringComparison.OrdinalIgnoreCase))
        {
            return nostrPublishHandler.Handle(nostrMessage);
        }
        else if (nostrMessage.Message.StartsWith("[\"CLOSE", StringComparison.OrdinalIgnoreCase))
        {
            return nostrCloseHandler.Handle(nostrMessage);
        }
        else
        {
            return Task.CompletedTask;
        }
    }

    public Task Disconnect(NostrMessage nostrMessage)
    {
        return Task.CompletedTask;
    }
}