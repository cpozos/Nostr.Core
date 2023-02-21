using Nostr.Core.Communication;
using Nostr.Core.Models;

namespace Nostr.Core.Interfaces;

public interface INostrRepo
{
    Task<NostrEventRequest[]> GetEvents(NostrFilterRequest? nostrFilters);
}