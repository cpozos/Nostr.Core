using Nostr.Core.Contracts;
using Nostr.Core.Models;
using Nostr.Core.Interfaces;
using System.Text.Json;

namespace Nostr.Core.EventHandlers;

public class NostrRequestEventHandler : INostrRequestEventHandler
{
    public NostrRequestEventHandler()
    {
    }

    public async Task Handle(NostrMessage nostrMessage, INostrConnection connection, INostrRepo nostrRepo)
    {
        try
        {
            // Parse json
            using var jsonDoc = JsonDocument.Parse(nostrMessage.Message);
            var json = jsonDoc.RootElement;
            string? subscriptionId = json[1].GetString();
            var filters = JsonSerializer.Deserialize<NostrFilterRequest>(json[2]);

            // Get events filtering by the filters
            var events = await nostrRepo.GetEvents(filters);
            await connection.SendMessage(nostrMessage with { Message = JsonSerializer.Serialize(events) }, CancellationToken.None);
        }
        catch (Exception ex)
        {
        }
    }
}