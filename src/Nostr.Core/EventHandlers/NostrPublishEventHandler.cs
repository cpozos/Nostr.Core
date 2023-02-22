using Nostr.Core.Models;
using Nostr.Core.Interfaces;
using System.Text.Json;
using Nostr.Core.Contracts;
using Nostr.Core.Persistence;
using System.Linq;

namespace Nostr.Core.EventHandlers;

public class NostrPublishEventHandler : INostrPublishEventHandler
{
    public async Task Handle(NostrMessage nostrMessage, INostrRepo nostrRepo)
    {
        try
        {
            using var jsonDocument = JsonDocument.Parse(nostrMessage.Message);
            var json = jsonDocument.RootElement;
            var nostrEvent = JsonSerializer.Deserialize<NostrEventRequest>(json[1].GetRawText());
            if (nostrEvent is null)
            {
                // TODO: handle it better ??
                throw new Exception("");
            }


            // Get all subscriptions
            var predicate = EventSpecification.FilterMatchesEvent(nostrEvent).Compile();
            var allSubscriptions = (await nostrRepo.GetAllSubscriptions());
            var allSubscriptionsToNotify = allSubscriptions
                .Where(s => s.Filters.Any(predicate));

            // Push notifications to subscribers whose filter (s) matches event.
            foreach (var subscription in allSubscriptionsToNotify)
            {
                // TODO send notification
                Console.WriteLine(subscription);
            }

            await nostrRepo.SaveEvent(nostrEvent);
        }
        catch (Exception ex)
        {

        }
    }
}