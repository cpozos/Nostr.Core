using System.Text.Json.Serialization;
using Nostr.Core.JsonConverters;

namespace Nostr.Core.Contracts;

public record NostrFilterRequest
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("ids")]
    public string[]? Ids { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("authors")]
    public string[]? Authors { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("kinds")]
    public int[]? Kinds { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("#e")]
    public string[]? EventId { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("#p")]
    public string[]? PublicKey { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("since")]
    [JsonConverter(typeof(UnixTimestampSecondsJsonConverter))]
    public DateTimeOffset? Since { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("until")]
    [JsonConverter(typeof(UnixTimestampSecondsJsonConverter))]
    public DateTimeOffset? Until { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("limit")]
    public int? Limit { get; set; }
}