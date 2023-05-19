using Microsoft.Azure.WebJobs;

namespace Common.Webjob.Extensions;

public static class WebJobsExtensions
{
    public static IWebJobsBuilder UseHostId(this IWebJobsBuilder builder)
    {
#if DEBUG
        return builder.UseHostId(Guid.NewGuid().ToString("N"));
#else
            return builder;
#endif
    }
}