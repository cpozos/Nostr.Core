using Nostr.Core.Contracts;
using Nostr.Core.Interfaces;
using Nostr.Core.Models;

namespace Nostr.Core.Persistence;

public class NostrRepo : INostrRepo
{
    public Task<IQueryable<NostrSubscription>> GetAllSubscriptions()
    {
        return Task.FromResult(new List<NostrSubscription>()
        {
            new()
            {
                Id = "Subscription1",
                Filters = new()
                {
                    new()
                    {
                        Ids = new string[] { "event1" }
                    }
                }
            },
            new()
            {
                Id = "Subscription2",
                Filters = new()
                {
                    new()
                    {
                        Ids = new string[] { "event2" }
                    }
                }
            },
        }.AsQueryable());
    }

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

    public Task SaveEvent(NostrEventRequest nostrEvent)
    {

        return Task.CompletedTask;
    }
}