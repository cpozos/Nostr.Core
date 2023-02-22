using Nostr.Core.Contracts;
using Nostr.Core.Models;

namespace Nostr.Core.Persistence;

public interface INostrRepo
{
    Task<NostrEventRequest[]> GetEvents(NostrFilterRequest? nostrFilters);
    Task RemoveFilters(NostrSubscription[] subscriptions);
}