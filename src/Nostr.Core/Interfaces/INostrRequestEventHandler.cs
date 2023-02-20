using Nostr.Core.DTOs;

namespace Nostr.Core.Interfaces;

internal interface INostrRequestEventHandler
{
    Task Handle(NostrMessage nostrMessage);
}