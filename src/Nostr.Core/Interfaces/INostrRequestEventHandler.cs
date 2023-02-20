using Nostr.Core.Models;

namespace Nostr.Core.Interfaces;

internal interface INostrRequestEventHandler
{
    Task Handle(NostrMessage nostrMessage);
}