using Azure.Storage.Queues.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Pong.AzureWebjob.Services;

namespace Pong.AzureWebjob;

public class Functions
{
    private const string FunctionName = "PongAzureWebjob";
    private const string TimeoutExpression = "00:00:30";
    
    private readonly IPongService _service;

    public Functions(IPongService service)
    {
        _service = service ?? throw new ArgumentNullException(nameof(service));
    }

    [Timeout(TimeoutExpression)]
    [FunctionName(FunctionName)]
    public async Task RunAsync(
        [QueueTrigger("%Settings:AzureQueueName%")] QueueMessage message,
        ILogger logger,
        CancellationToken cancellationToken)
    {
        await _service.PongAsync(message.MessageText, cancellationToken);
    }
}