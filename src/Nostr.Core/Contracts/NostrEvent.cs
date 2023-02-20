using System.Text.Json.Serialization;

namespace Nostr.Core.Models;

public record NostrEvent
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("pubkey")]
    public string PublicKey { get; set; }

    [JsonPropertyName("created_at")]
    public DateTimeOffset? CreatedAt { get; set; }

    [JsonPropertyName("kind")]
    public int Kind { get; set; }

    [JsonPropertyName("tags")]
    public List<NostrEventTag> Tags { get; set; }

    [JsonPropertyName("content")]
    public string Content { get; set; }

    [JsonPropertyName("sig")]
    public string Signature { get; set; }
}