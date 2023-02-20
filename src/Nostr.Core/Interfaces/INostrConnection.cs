using Nostr.Core.DTOs;

public interface INostrConnection
{
    public string Id { get; set; }
    public Task SendMessage(NostrMessage nostrMessage, CancellationToken cancellationToken);
}