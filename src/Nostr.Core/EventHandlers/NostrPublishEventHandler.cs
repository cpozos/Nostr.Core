using Nostr.Core.Models;
using Nostr.Core.Interfaces;
using System.Text.Json;
using Nostr.Core.Contracts;
using Nostr.Core.Persistence;
using System.Linq;
using LinqKit;

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
            var allSubscriptions = await nostrRepo.GetAllSubscriptions();
            var allSubscriptionsToNotify = allSubscriptions.AsEnumerable()
                .Where(s => s.Filters.Any(nostrFilter =>
                {
                    bool t1 = nostrFilter.Ids == null || nostrFilter.Ids.Any(id => nostrEvent.Id.StartsWith(id));
                    bool t2 = nostrFilter.Kinds == null || nostrFilter.Kinds.Contains(nostrEvent.Kind);
                    bool t3 = nostrFilter.Since == null || nostrEvent.CreatedAt > nostrFilter.Since;
                    bool t4 = nostrFilter.Until == null || nostrEvent.CreatedAt < nostrFilter.Until;
                    bool t5 = nostrFilter.Authors == null || nostrFilter.Authors.Any(s => nostrEvent.PublicKey.StartsWith(s));
                    bool t6 = nostrFilter.EventId == null ||
                        nostrEvent.Tags.Any(tag => tag.TagIdentifier == "e" && nostrFilter.EventId.Contains(tag.Data[0]));

                    bool t7 = nostrFilter.PublicKey == null ||
                        nostrEvent.Tags.Any(tag => tag.TagIdentifier == "p" && nostrFilter.PublicKey.Contains(tag.Data[0]));

                    return t1 && t2 && t3 && t4 && t5 && t6 && t7;
                })).ToList();

            // Push notifications to subscribers whose filter (s) matches event.
            var message = nostrMessage with { Message = JsonSerializer.Serialize(nostrEvent) };

            allSubscriptionsToNotify.GroupBy(x => x.ConnectionId).ForEach(async i =>
            {
                var connection = await nostrRepo.GetConnection(i.Key);
                await connection.SendMessage(message, CancellationToken.None);
            });

            await nostrRepo.SaveEvent(nostrEvent);
        }
        catch (Exception ex)
        {
        }
    }
}