using Nostr.Core.JsonConverters;
using System.Text.Json.Serialization;

namespace Nostr.Core.Models;

public record NostrEventRequest
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("pubkey")]
    public string PublicKey { get; set; } = string.Empty;

    [JsonPropertyName("created_at")]
    [JsonConverter(typeof(UnixTimestampSecondsJsonConverter))]
    public DateTimeOffset? CreatedAt { get; set; }

    [JsonPropertyName("kind")]
    public int Kind { get; set; }

    [JsonPropertyName("tags")]
    public List<NostrEventTag> Tags { get; set; } = new ();

    [JsonPropertyName("content")]
    public string Content { get; set; } = string.Empty;

    [JsonPropertyName("sig")]
    public string Signature { get; set; } = string.Empty;
}