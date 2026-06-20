using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace BuildingBlocks.Logging;

public static class LoggingExtensions
{
    public static ConfigureHostBuilder UseCustomLogging(this ConfigureHostBuilder host, string applicationName)
    {
        host.UseSerilog((context, loggerConfiguration) =>
        {
            loggerConfiguration
                .MinimumLevel.Information()
                .Enrich.FromLogContext()
                .Enrich.WithProperty("ApplicationName", applicationName)
                .Enrich.WithMachineName()
                .Enrich.WithProcessId()
                .WriteTo.Console()
                .WriteTo.Seq("http://seq:80");
        });

        return host;
    }

    public static IApplicationBuilder UseCorrelationId(this IApplicationBuilder app)
    {
        return app.UseMiddleware<CorrelationIdMiddleware>();
    }
}