using Nostr.Core;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddTransient<NostrMessagePropagator>();
builder.Services.AddTransient<NostrMiddleware>();

var app = builder.Build();
app.UseWebSockets();
app.UseMiddleware<NostrMiddleware>();
app.Run();