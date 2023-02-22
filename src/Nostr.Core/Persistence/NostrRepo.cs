using Nostr.Core.Contracts;
using Nostr.Core.Interfaces;
using Nostr.Core.Models;

namespace Nostr.Core.Persistence;

public class NostrRepo : INostrRepo
{
    public Task<NostrEventRequest[]> GetEvents(NostrFilterRequest? nostrFilters)
    {
        return Task.FromResult(
            new List<NostrEventRequest>().AsQueryable()
                .Where(nostrFilters)
                .ToArray());
    }

    public Task RemoveFilters(NostrSubscription[] subscriptions)
    {
        return Task.CompletedTask;
    }
}