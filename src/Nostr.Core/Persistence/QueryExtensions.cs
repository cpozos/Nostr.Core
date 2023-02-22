using Nostr.Core.Contracts;

namespace Nostr.Core.Persistence;

public static class QueryExtensions
{
    public static IQueryable<NostrEventRequest> Where(this IQueryable<NostrEventRequest> query, NostrFilterRequest? filters)
    {
        if (filters is null)
        {
            return query;
        }

        return query.Where(EventSpecification.NostrEventMatchesFilter(filters));
    }
}