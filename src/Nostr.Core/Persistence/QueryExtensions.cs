using Nostr.Core.Communication;
using Nostr.Core.Models;

namespace Nostr.Core.Persistence;

public static class QueryExtensions
{
    public static IQueryable<NostrEventRequest> Where(this IQueryable<NostrEventRequest> query, NostrFilterRequest? filters)
    {
        if (filters is null)
        {
            return query;
        }

        if (filters.Ids?.Any() is true)
        {
            query = query.Where(e => filters.Ids.Any(s => e.Id.StartsWith(s)));
        }

        if (filters.Kinds?.Any() is true)
        {
            query = query.Where(e => filters.Kinds.Contains(e.Kind));
        }

        if (filters.Since != null)
        {
            query = query.Where(e => e.CreatedAt > filters.Since);
        }

        if (filters.Until != null)
        {
            query = query.Where(e => e.CreatedAt < filters.Until);
        }

        var authors = filters.Authors?.Where(s => !string.IsNullOrWhiteSpace(s))?.ToArray();
        if (authors?.Any() is true)
        {
            query = query.Where(e => authors.Any(s => e.PublicKey.StartsWith(s)));
        }

        if (filters.EventId?.Any() is true)
        {
            query = query.Where(e =>
                e.Tags.Any(tag => tag.TagIdentifier == "e" && filters.EventId.Contains(tag.Data[0])));
        }

        if (filters.PublicKey?.Any() is true)
        {
            query = query.Where(e =>
                e.Tags.Any(tag => tag.TagIdentifier == "p" && filters.PublicKey.Contains(tag.Data[0])));
        }

        return query;
    }
}