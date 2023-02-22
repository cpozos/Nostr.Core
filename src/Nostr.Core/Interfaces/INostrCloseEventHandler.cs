using Nostr.Core.Models;
using Nostr.Core.Persistence;

namespace Nostr.Core.Interfaces;

public interface INostrCloseEventHandler
{
    Task Handle(NostrMessage nostrMessage, INostrRepo repo);
}