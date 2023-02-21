using Nostr.Core.Models;

namespace Nostr.Core.Interfaces;

public interface INostrCloseEventHandler
{
    Task Handle(NostrMessage nostrMessage);
}