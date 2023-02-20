using Nostr.Core.DTOs;

namespace Nostr.Core.Interfaces;

public interface INostrPublishEventHandler
{
    Task Handle(NostrMessage nostrMessage);
}