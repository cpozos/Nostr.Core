using Nostr.Core.Models;
using Nostr.Core.Persistence;

namespace Nostr.Core.Interfaces;

internal interface INostrRequestEventHandler
{
    Task Handle(NostrMessage nostrMessage, INostrConnection connection, INostrRepo nostrEventRepo);
}