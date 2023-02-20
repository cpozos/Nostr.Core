using Nostr.Core.Models;

namespace Nostr.Core.Interfaces;

public interface INostrManager
{
    Task<NostrEventRequest[]> ConfigureFilters(NostrFilters filters);
}