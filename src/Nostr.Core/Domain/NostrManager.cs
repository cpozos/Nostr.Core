using Nostr.Core.DTOs;
using Nostr.Core.Interfaces;
using Nostr.Core.Models;

namespace Nostr.Core.Domain;

internal class NostrManager : INostrManager
{
    public Task<NostrEvent[]> ConfigureFilters(AddUpdateNostrFilters context)
    {
        return Task.FromResult(Array.Empty<NostrEvent>());
    }
}