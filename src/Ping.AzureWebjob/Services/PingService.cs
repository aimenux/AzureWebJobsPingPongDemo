using Common.Webjob.Helpers;
using Microsoft.Extensions.Logging;

namespace Ping.AzureWebjob.Services;

public class PingService : IPingService
{
    private readonly IAzureQueueProvider _provider;
    private readonly ILogger<PingService> _logger;

    public PingService(IAzureQueueProvider provider, ILogger<PingService> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _provider = provider ?? throw new ArgumentNullException(nameof(provider));
    }

    public async Task PingAsync(CancellationToken cancellationToken)
    {
        await _provider.EnqueueMessageAsync(GetPingMessage(), cancellationToken);
    }
    
    private static string GetPingMessage() => $"Ping-{DateTime.Now:yyMMddHHmmss}";
}