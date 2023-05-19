using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Common.Webjob.Extensions;

public static class LoggingExtensions
{
    public static void AddApplicationInsights(this ILoggingBuilder loggingBuilder, IConfiguration configuration)
    {
        var instrumentationKey = configuration["Settings:ApplicationInsights:instrumentationKey"];
        if (!string.IsNullOrWhiteSpace(instrumentationKey))
        {
            loggingBuilder.AddApplicationInsightsWebJobs(options => options.InstrumentationKey = instrumentationKey);
        }
    }
}