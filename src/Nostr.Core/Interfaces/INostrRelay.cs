namespace Nostr.Core.Interfaces;

public interface INostrRelay
{
    void AddConnection(INostrConnection connection);
    void RemoveConnection(INostrConnection connection);
    INostrConnection[] GetConnections(params string[] ids);
}