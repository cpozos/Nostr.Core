using Nostr.Core.DTOs;
using Nostr.Core.Interfaces;

namespace Nostr.Core.EventHandlers;

internal class NostrCloseEventHandler : INostrCloseEventHandler
{
    public Task Handle(NostrMessage nostrMessage)
    {
        return Task.CompletedTask;
    }
}