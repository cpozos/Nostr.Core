namespace Nostr.Core.Interfaces;

internal interface INostrResponsesDispatcher
{
    Task StartReadMessages(CancellationToken cancellationToken);
}