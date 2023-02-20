using Nostr.Core.Domain;
using Nostr.Core.DTOs;
using Nostr.Core.Interfaces;
using System.Text.Json;

namespace Nostr.Core.EventHandlers;

public class NostrRequestEventHandler : INostrRequestEventHandler
{
    private readonly INostrManager _manager = new NostrManager();

    public Task Handle(NostrMessage nostrMessage)
    {
        try
        {
            using var jsonDoc = JsonDocument.Parse(nostrMessage.Message);
            var json = jsonDoc.RootElement;

            // Parse filters
            var action = new AddUpdateNostrFilters(nostrMessage.ConnectionId, json[1].GetString());
            for (int i = 2; i < json.GetArrayLength(); i++)
            {
                action.AddFilter(json[i]);
            }

            // Send filters update request
            var events = _manager.ConfigureFilters(action);
        }
        catch (Exception ex)
        {
        }

        return Task.CompletedTask;
    }
}