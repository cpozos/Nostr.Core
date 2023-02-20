namespace Nostr.Core.Interfaces;

public interface INostrRelay
{
    void AddConnection(INostrConnection connection)
}