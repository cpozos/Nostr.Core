using Nostr.Core.Domain;
using Nostr.Core.Models;
using Nostr.Core.Interfaces;
using System.Text.Json;

namespace Nostr.Core.EventHandlers;

public class NostrRequestEventHandler : INostrRequestEventHandler
{
    private readonly INostrManager _manager = new NostrManager();

    public async Task Handle(NostrMessage nostrMessage)
    {
        try
        {
            using var jsonDoc = JsonDocument.Parse(nostrMessage.Message);
            var json = jsonDoc.RootElement;

            // Parse filters
            var filters = new NostrFilters(nostrMessage.ConnectionId, json[1].GetString());
            for (int i = 2; i < json.GetArrayLength(); i++)
            {
                filters.AddFilter(json[i]);
            }

            // Send filters update request
            var events = await _manager.ConfigureFilters(filters);
        }
        catch (Exception ex)
        {
        }
    }
}