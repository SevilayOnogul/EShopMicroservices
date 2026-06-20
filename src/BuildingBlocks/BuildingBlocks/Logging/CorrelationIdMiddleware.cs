using Microsoft.AspNetCore.Http;
using Serilog.Context;

namespace BuildingBlocks.Logging;

public class CorrelationIdMiddleware
{
    private readonly RequestDelegate _next;
    private const string CorrelationIdHeaderKey = "X-Correlation-ID";

    public CorrelationIdMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (!context.Request.Headers.TryGetValue(CorrelationIdHeaderKey, out var correlationId))
        {
            correlationId = Guid.NewGuid().ToString();
        }

        context.Response.Headers[CorrelationIdHeaderKey] = correlationId;

        using (LogContext.PushProperty("CorrelationId", correlationId.ToString()))
        {
            await _next(context);
        }
    }
}