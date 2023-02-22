using Nostr.Core.Models;
using Nostr.Core.Interfaces;

namespace Nostr.Core.EventHandlers;

public class NostrCloseEventHandler : INostrCloseEventHandler
{
    public async Task Handle(NostrMessage nostrMessage, INostrRepo repo)
    {
        // Get subscriptions for given connection
        var subscriptions = new List<NostrSubscription>();

        // Remove subscriptions' filters
        await repo.RemoveFilters(subscriptions.ToArray());
    }
}