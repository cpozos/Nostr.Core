using Nostr.Core.Contracts;
using Nostr.Core.Models;
using System.Collections.Concurrent;
using System.Net.WebSockets;

public interface INostrConnection : IDisposable
{
    public string Id { get; set; }
    public bool IsConnectionOpen { get; }
    public Task SendMessage(NostrMessage nostrMessage, CancellationToken cancellationToken);
    public Task<(WebSocketReceiveResult Result, string Message)> ReceiveMessage();
    public Task Disconnect();
}