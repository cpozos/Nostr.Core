using Nostr.Core.Contracts;

namespace Nostr.Core.Models;

public class NostrSubscription
{
    public string Id { get; set; } = string.Empty;

    public List<NostrFilterRequest> Filters { get; set; } = new();
}