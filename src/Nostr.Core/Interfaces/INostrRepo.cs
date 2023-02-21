using Nostr.Core.Contracts;

namespace Nostr.Core.Interfaces;

public interface INostrRepo
{
    Task<NostrEventRequest[]> GetEvents(NostrFilterRequest? nostrFilters);
}