using Nostr.Core.JsonConverters;
using System.Text.Json.Serialization;

namespace Nostr.Core.Contracts;

[JsonConverter(typeof(NostrEventTagJsonConverter))]
public class NostrEventTag
{
    public string Id { get; set; } = string.Empty;

    public string TagIdentifier { get; set; } = string.Empty;

    public List<string> Data { get; set; } = new();
}