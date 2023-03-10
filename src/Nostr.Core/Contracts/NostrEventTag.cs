namespace Nostr.Core.Contracts;

public record NostrEventTag
{
    public string Id { get; set; }

    public string TagIdentifier { get; set; }

    public IReadOnlyList<string> Data { get; set; } = Array.Empty<string>();
}