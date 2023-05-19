using Azure.Storage.Queues;
using Microsoft.Extensions.Logging;

namespace Common.Webjob.Helpers;

public interface IAzureQueueProvider
{
    Task EnqueueMessageAsync(string message, CancellationToken cancellationToken = default);
}

public class AzureQueueProvider : IAzureQueueProvider
{
    private static readonly TimeSpan NeverExpire = TimeSpan.FromSeconds(-1);
    
    private readonly QueueClient _client;
    private readonly ILogger<AzureQueueProvider> _logger;

    public AzureQueueProvider(QueueClient client, ILogger<AzureQueueProvider> logger)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task EnqueueMessageAsync(string message, CancellationToken cancellationToken)
    {
        try
        {
            var response = await _client.SendMessageAsync(message, timeToLive: NeverExpire, cancellationToken: cancellationToken);
            _logger.LogInformation("Succeeded to add message '{message}' with id {id}", message, response.Value.MessageId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to add message '{message}'", message);
        }
    }
}