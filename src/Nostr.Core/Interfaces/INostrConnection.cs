using Nostr.Core.Models;
using System.Net.WebSockets;

public interface INostrConnection
{
    public string Id { get; set; }
    public bool IsConnectionOpen { get; }
    public Task SendMessage(NostrMessage nostrMessage, CancellationToken cancellationToken);
    public Task<(WebSocketReceiveResult Result, string Message)> ReceiveMessage();
}