using Microsoft.Extensions.Logging;

namespace Pong.AzureWebjob.Services;

public class PongService : IPongService
{
    private static readonly TimeSpan Delay = TimeSpan.FromSeconds(1);
    
    private readonly ILogger<PongService> _logger;

    public PongService(ILogger<PongService> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task PongAsync(string message, CancellationToken cancellationToken)
    {
        await Task.Delay(Delay, cancellationToken);
        
        using var scope = _logger.BeginScope(new Dictionary<string, string>
        {
            ["PingMessage"] = message,
            ["PongMessage"] = GetPongMessage()
        });
        
        _logger.LogInformation("Pong succeeded for '{message}'", message);
    }
    
    private static string GetPongMessage() => $"Pong-{DateTime.Now:yyMMddHHmmss}";
}