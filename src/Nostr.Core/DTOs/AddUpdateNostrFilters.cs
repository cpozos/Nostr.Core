using Nostr.Core.Communication;
using System.Text;
using System.Text.Json;

namespace Nostr.Core.DTOs;

public class AddUpdateNostrFilters
{
    public string ConnectionId { get; init; }
    public string SubscriptionId { get; init; }
    public Dictionary<string, NostrFilter> Filters { get; init; } = new();

    public AddUpdateNostrFilters(string? connectionId, string? subscriptionId)
    {
        ArgumentNullException.ThrowIfNullOrEmpty(connectionId);
        ArgumentNullException.ThrowIfNullOrEmpty(subscriptionId);
        ConnectionId = connectionId;
        SubscriptionId = subscriptionId;
    }

    public void AddFilter(JsonElement jsonElement)
    {
        string txt = jsonElement.GetRawText();
        Filters.Add(ToHex(ComputeSha256Hash(txt)), JsonSerializer.Deserialize<NostrFilter>(jsonElement));
    }
    public void AddFilter(NostrFilter nostrFilter)
    {
        string txt = JsonSerializer.Serialize(nostrFilter);
        Filters.Add(ToHex(ComputeSha256Hash(txt)), nostrFilter);
    }


    public static byte[] ComputeSha256Hash(string rawData)
    {
        // Create a SHA256
        using var sha256 = System.Security.Cryptography.SHA256.Create();
        // ComputeHash - returns byte array
        return sha256.ComputeHash(Encoding.UTF8.GetBytes(rawData));
    }

    public static string ToHex(Span<byte> bytes)
    {
        return Convert.ToHexString(bytes).ToLowerInvariant();
    }
}