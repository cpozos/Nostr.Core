using Nostr.Core;
using Nostr.Core.Interfaces;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<INostrRelay, NostrRelay>();
builder.Services.AddTransient<NostrMessagePropagator>();
builder.Services.AddTransient<NostrMiddleware>();

var app = builder.Build();
app.UseWebSockets();
app.UseMiddleware<NostrMiddleware>();
app.Run();