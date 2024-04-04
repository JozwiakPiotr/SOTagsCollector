using Serilog;

namespace SOTagsCollector.API.Installers;

public static class LoggingConfigurer
{
    public static WebApplicationBuilder ConfigureLogging(this WebApplicationBuilder builder)
    {
        var config = new LoggerConfiguration()
            #if DEBUG
            .MinimumLevel.Debug()
            #else
            .MinimumLevel.Warning()
            #endif
            .WriteTo.File(
                "logs/log.txt",
                rollingInterval: RollingInterval.Day,
                rollOnFileSizeLimit: true,
                fileSizeLimitBytes: 100_000_000)
            .WriteTo.Console()
            .CreateLogger();
        Log.Logger = config;
        return builder;
    }
}