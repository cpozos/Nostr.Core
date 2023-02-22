using Nostr.Core.Contracts;
using System.Linq;

namespace Nostr.Core.Persistence;

public static class EnumerableExtensions
{
    public static IEnumerable<NostrEventRequest> Where(this IEnumerable<NostrEventRequest> query, NostrFilterRequest? filters)
    {
        if (filters is null)
        {
            return query;
        }

        return query.Where(EventSpecification.NostrEventMatchesFilter(filters).Compile());
    }
}