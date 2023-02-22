using Nostr.Core.Models;

namespace Nostr.Core.Interfaces;

public interface INostrPublishEventHandler
{
    Task Handle(NostrMessage nostrMessage, INostrRepo nostrRepo);
}