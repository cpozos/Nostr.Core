using Nostr.Core.DTOs;
using Nostr.Core.EventHandlers;
using Nostr.Core.Interfaces;
using System.Runtime.InteropServices;

namespace Nostr.Core;

public class NostrMessageHandler
{
    private readonly List<INostrCloseEventHandler> nostrEventHandlers = new ()
    {
        new NostrCloseEventHandler(),
    };

    private readonly List<INostrPublishEventHandler> nostrPublishHandlers = new()
    {
        new NostrPublishEventHandler(),
    };

    private readonly List<INostrRequestEventHandler> nostrRequestHandlers = new()
    {
        new NostrRequestEventHandler(),
    };

    public Task HandleMessage(NostrMessage nostrMessage)
    {
        ParallelQuery<Task> handlersTasks;

        if (nostrMessage.Message.StartsWith("[\"REQ", StringComparison.OrdinalIgnoreCase))
        {
            handlersTasks = nostrRequestHandlers.AsParallel().Select(async x => await x.Handle(nostrMessage));
        }
        else if (nostrMessage.Message.StartsWith("[\"EVENT", StringComparison.OrdinalIgnoreCase))
        {
            handlersTasks = nostrPublishHandlers.AsParallel().Select(async x => await x.Handle(nostrMessage));
        }
        else if (nostrMessage.Message.StartsWith("[\"CLOSE", StringComparison.OrdinalIgnoreCase))
        {
            handlersTasks = nostrEventHandlers.AsParallel().Select(async x => await x.Handle(nostrMessage));
        }
        else
        {
            return Task.CompletedTask;
        }

        return Task.WhenAll(handlersTasks);
    }

    public Task Disconnect(NostrMessage nostrMessage)
    {
        return Task.CompletedTask;
    }
}