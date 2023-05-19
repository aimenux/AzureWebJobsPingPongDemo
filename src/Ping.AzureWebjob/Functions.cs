using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Ping.AzureWebjob.Services;

namespace Ping.AzureWebjob;

public class Functions
{
    private const string FunctionName = "PingAzureWebjob";
    private const string TimeoutExpression = "00:00:30";

    private readonly IPingService _service;

    public Functions(IPingService service)
    {
        _service = service ?? throw new ArgumentNullException(nameof(service));
    }

    [Timeout(TimeoutExpression)]
    [FunctionName(FunctionName)]
    public async Task RunAsync(
        [TimerTrigger("%Settings:CronExpression%", RunOnStartup = false)]
        TimerInfo timer,
        CancellationToken cancellationToken,
        ILogger logger)
    {
        await _service.PingAsync(cancellationToken);
    }
}