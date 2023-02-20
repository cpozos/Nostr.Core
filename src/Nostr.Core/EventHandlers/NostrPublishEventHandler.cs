using Nostr.Core.DTOs;
using Nostr.Core.Interfaces;

namespace Nostr.Core.EventHandlers;

public class NostrPublishEventHandler : INostrPublishEventHandler
{
    public Task Handle(NostrMessage nostrMessage)
    {
        return Task.CompletedTask;
    }
}