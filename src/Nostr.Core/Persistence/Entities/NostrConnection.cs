
namespace Nostr.Core.Persistence.Entities;

public class NostrConnectionEF
{
    required public Guid Id { get; set; }
    public List<NostrSubscriptionEF> Subscriptions { get; set; }
}

public class NostrSubscriptionEF
{
    required public string Id { get; set; }

    public List<NostrFilterEF> Filters { get; set; } = new();
}

public class NostrFilterEF
{
    required public string Id { get; set; }
}