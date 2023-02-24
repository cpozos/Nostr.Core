using Nostr.Core;
using Nostr.Core.Interfaces;
using Nostr.Core.Persistence;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<INostrRelay, NostrRelay>();
builder.Services.AddSingleton<INostrRepo, NostrRepo>();
builder.Services.AddTransient<NostrMessagePropagator>();
builder.Services.AddTransient<NostrMiddleware>();

var app = builder.Build();
app.UseWebSockets();
app.UseMiddleware<NostrMiddleware>();
app.Run();