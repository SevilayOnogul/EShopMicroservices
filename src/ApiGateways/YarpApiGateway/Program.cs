using Microsoft.AspNetCore.RateLimiting;
using BuildingBlocks.Logging;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseCustomLogging("YarpApiGateway");
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

builder.Services.AddRateLimiter(rateLimiterOptions =>
{
    rateLimiterOptions.AddFixedWindowLimiter("fixed", options =>
    {
        options.Window = TimeSpan.FromSeconds(10);
        options.PermitLimit = 5;
    });
});

var app = builder.Build();
app.UseCorrelationId();

app.UseRateLimiter();

app.MapReverseProxy();

app.Run();