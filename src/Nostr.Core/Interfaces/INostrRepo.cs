using Nostr.Core.Contracts;
using Nostr.Core.Models;

namespace Nostr.Core.Interfaces;

public interface INostrRepo
{
    Task<NostrEventRequest[]> GetEvents(NostrFilterRequest? nostrFilters);
    Task RemoveFilters(NostrSubscription[] subscriptions);
}