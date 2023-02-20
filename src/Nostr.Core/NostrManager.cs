using Nostr.Core.Models;
using Nostr.Core.Interfaces;

namespace Nostr.Core;

internal class NostrManager : INostrManager
{
    public Task<NostrEventRequest[]> ConfigureFilters(NostrFilters context)
    {
        return Task.FromResult(Array.Empty<NostrEventRequest>());
    }
}