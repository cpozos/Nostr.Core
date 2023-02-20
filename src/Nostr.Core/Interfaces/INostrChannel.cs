using Nostr.Core.Models;

namespace Nostr.Core.Interfaces;

internal interface INostrChannel
{
    Task WriteMessage(NostrMessage nostrMessage);

    Task<NostrMessage> ReadMessage();
}