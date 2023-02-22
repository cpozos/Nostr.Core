using LinqKit;
using Nostr.Core.Contracts;
using Nostr.Core.Models;
using System.Linq.Expressions;

namespace Nostr.Core.Persistence;

public static class EventSpecification
{
    public static Expression<Func<NostrFilterRequest, bool>> FilterMatchesEvent(NostrEventRequest e)
    {
        var predicate = PredicateBuilder.New<NostrFilterRequest>(true);
        predicate = predicate.And(nostrFilter => nostrFilter.Ids == null || nostrFilter.Ids.Any(id => e.Id.StartsWith(id)));
        predicate = predicate.And(nostrFilter => nostrFilter.Kinds == null || nostrFilter.Kinds.Contains(e.Kind));
        predicate = predicate.And(nostrFilter => nostrFilter.Since == null || e.CreatedAt > nostrFilter.Since);
        predicate = predicate.And(nostrFilter => nostrFilter.Until == null || e.CreatedAt < nostrFilter.Until);
        predicate = predicate.And(nostrFilter => nostrFilter.Authors == null || nostrFilter.Authors.Any(s => e.PublicKey.StartsWith(s)));
        predicate = predicate.And(nostrFilter => nostrFilter.EventId == null ||
            e.Tags.Any(tag => tag.TagIdentifier == "e" && nostrFilter.EventId.Contains(tag.Data[0])));
        predicate = predicate.And(nostrFilter => nostrFilter.PublicKey == null ||
            e.Tags.Any(tag => tag.TagIdentifier == "p" && nostrFilter.PublicKey.Contains(tag.Data[0])));
        return predicate;
    }

    public static Expression<Func<NostrEventRequest, bool>> NostrEventMatchesFilter(NostrFilterRequest nostrFilter)
    {
        var predicate = PredicateBuilder.New<NostrEventRequest>(true);

        if (nostrFilter.Ids?.Any() is true)
        {
            predicate = predicate.And(e => nostrFilter.Ids.Any(id => e.Id.StartsWith(id)));
        }

        if (nostrFilter.Kinds?.Any() is true)
        {
            predicate = predicate.And(e => nostrFilter.Kinds.Contains(e.Kind));
        }

        if (nostrFilter.Since != null)
        {
            predicate = predicate.And(e => e.CreatedAt > nostrFilter.Since);
        }

        if (nostrFilter.Until != null)
        {
            predicate = predicate.And(e => e.CreatedAt < nostrFilter.Until);
        }

        var authors = nostrFilter.Authors?.Where(s => !string.IsNullOrWhiteSpace(s))?.ToArray();
        if (authors?.Any() is true)
        {
            predicate = predicate.And(e => authors.Any(s => e.PublicKey.StartsWith(s)));
        }

        if (nostrFilter.EventId?.Any() is true)
        {
            predicate = predicate.And(e =>
                e.Tags.Any(tag => tag.TagIdentifier == "e" && nostrFilter.EventId.Contains(tag.Data[0])));
        }

        if (nostrFilter.PublicKey?.Any() is true)
        {
            predicate = predicate.And(e =>
                e.Tags.Any(tag => tag.TagIdentifier == "p" && nostrFilter.PublicKey.Contains(tag.Data[0])));
        }

        return predicate;
    }
}