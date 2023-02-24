using Nostr.Core.Contracts;
using Nostr.Core.Models;

namespace Nostr.Core.Interfaces;

public interface INostrRepo
{
    void AddConnection(INostrConnection connection);
    void RemoveConnection(INostrConnection connection);
    Task<INostrConnection?> GetConnection(string connectionId);
    Task<IQueryable<NostrSubscription>> GetAllSubscriptions();
    Task<NostrEventRequest[]> GetEvents(NostrFilterRequest? nostrFilters);
    Task RemoveFilters(NostrSubscription[] subscriptions);
    Task SaveEvent(NostrEventRequest nostrEvent);
    Task UpsertSubscription(NostrSubscription nostrSubscription);
}