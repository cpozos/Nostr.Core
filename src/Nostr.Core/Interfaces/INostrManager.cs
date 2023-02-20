using Nostr.Core.DTOs;
using Nostr.Core.Models;

namespace Nostr.Core.Interfaces;

public interface INostrManager
{
    Task<NostrEvent[]> ConfigureFilters(AddUpdateNostrFilters context);
}