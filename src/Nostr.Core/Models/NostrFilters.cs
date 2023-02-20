using Nostr.Core.Communication;
using Nostr.Core.Extensions;
using System.Text.Json;

namespace Nostr.Core.Models;

public class NostrFilters
{
    public string ConnectionId { get; init; }
    public string SubscriptionId { get; init; }
    public Dictionary<string, NostrFilterRequest> Filters { get; init; } = new();

    public NostrFilters(string? connectionId, string? subscriptionId)
    {
        ArgumentNullException.ThrowIfNullOrEmpty(connectionId);
        ArgumentNullException.ThrowIfNullOrEmpty(subscriptionId);
        ConnectionId = connectionId;
        SubscriptionId = subscriptionId;
    }

    public void AddFilter(JsonElement jsonElement)
    {
        string txt = jsonElement.GetRawText();
        Filters.Add(txt.ComputeSha256Hash().AsSpan().ToHex(), JsonSerializer.Deserialize<NostrFilterRequest>(jsonElement));
    }

    public void AddFilter(NostrFilterRequest nostrFilter)
    {
        string txt = JsonSerializer.Serialize(nostrFilter);
        Filters.Add(txt.ComputeSha256Hash().AsSpan().ToHex(), nostrFilter);
    }
}