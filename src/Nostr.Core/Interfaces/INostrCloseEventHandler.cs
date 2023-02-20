using Nostr.Core.DTOs;

namespace Nostr.Core.Interfaces;

public interface INostrCloseEventHandler
{
    Task Handle(NostrMessage nostrMessage);
}