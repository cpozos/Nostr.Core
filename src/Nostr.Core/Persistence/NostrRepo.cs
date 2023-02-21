using Nostr.Core.Contracts;
using Nostr.Core.Interfaces;

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
}