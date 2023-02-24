using Nostr.Core.Contracts;
using Nostr.Core.Models;
using Nostr.Core.Interfaces;
using System.Text.Json;

namespace Nostr.Core.EventHandlers;

public class NostrRequestEventHandler : INostrRequestEventHandler
{
    public async Task Handle(NostrMessage nostrMessage, INostrConnection connection, INostrRepo nostrRepo)
    {
        try
        {
            // Parse json
            using var jsonDoc = JsonDocument.Parse(nostrMessage.Message);
            var json = jsonDoc.RootElement;
            string? subscriptionId = json[1].GetString();
            if (string.IsNullOrWhiteSpace(subscriptionId))
            {
                throw new Exception("Error");
            }

            var filters = JsonSerializer.Deserialize<NostrFilterRequest>(json[2]);
            if (filters is null)
            {
                throw new Exception("Error");
            }

            // Updates subscription
            var subscription = new NostrSubscription
            {
                Id = subscriptionId,
                ConnectionId = connection.Id,
            };
            subscription.Filters.Add(filters);
            await nostrRepo.UpsertSubscription(subscription);

            // Get events filtering by the filters
            var matchedEvents = await nostrRepo.GetEvents(filters);
            await connection.SendMessage(nostrMessage with { Message = JsonSerializer.Serialize(matchedEvents) }, CancellationToken.None);
        }
        catch (Exception ex)
        {
        }
    }
}