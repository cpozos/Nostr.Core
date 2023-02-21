using Nostr.Core.Communication;
using Nostr.Core.Extensions;
using System.Text.Json;

namespace Nostr.Core.Models;

public class NostrFilters
{
    public string ConnectionId { get; init; }
    public string SubscriptionId { get; init; }

    private readonly Dictionary<string, NostrFilterRequest> _filters = new();

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
        _filters.Add(txt.ComputeSha256Hash().AsSpan().ToHex(), JsonSerializer.Deserialize<NostrFilterRequest>(jsonElement));
    }

    public void AddFilter(NostrFilterRequest nostrFilter)
    {
        string txt = JsonSerializer.Serialize(nostrFilter);
        _filters.Add(txt.ComputeSha256Hash().AsSpan().ToHex(), nostrFilter);
    }

    public IEnumerable<NostrFilterRequest> Filters => _filters.Values.AsEnumerable();
}