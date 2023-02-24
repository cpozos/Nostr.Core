using Nostr.Core.Contracts;
using Nostr.Core.Interfaces;
using Nostr.Core.Models;
using System.Collections.Concurrent;

namespace Nostr.Core.Persistence;

public class NostrRepo : INostrRepo
{
    private static ConcurrentDictionary<string, INostrConnection> Connections = new();
    private static ConcurrentDictionary<string, NostrSubscription> ConnectionToSubscriptions = new();

    public void AddConnection(INostrConnection connection)
    {
        Connections.TryAdd(connection.Id, connection);
    }

    public void RemoveConnection(INostrConnection connection)
    {
        Connections.Remove(connection.Id, out var _);
    }

    public Task<INostrConnection?> GetConnection(string connectionId)
    {
        Connections.TryGetValue(connectionId, out INostrConnection? value);
        return Task.FromResult(value);
    }

    // SUBSCRIPTIONS
    public Task UpsertSubscription(NostrSubscription nostrSubscription)
    {
        ConnectionToSubscriptions.AddOrUpdate(
            nostrSubscription.ConnectionId + nostrSubscription.Id,
            nostrSubscription, (_, _) => nostrSubscription);

        return Task.CompletedTask;
    }

    public Task<IQueryable<NostrSubscription>> GetAllSubscriptions()
    {
        return Task.FromResult(ConnectionToSubscriptions.Select(x => x.Value).AsQueryable());
    }

    // EVENTS

    public Task SaveEvent(NostrEventRequest nostrEvent)
    {
        return Task.CompletedTask;
    }

    public Task<NostrEventRequest[]> GetEvents(NostrFilterRequest? nostrFilters)
    {
        return Task.FromResult(
            new List<NostrEventRequest>().AsQueryable()
                .Where(nostrFilters)
                .ToArray());
    }


    // FILTERS
    public Task RemoveFilters(NostrSubscription[] subscriptions)
    {
        return Task.CompletedTask;
    }
}