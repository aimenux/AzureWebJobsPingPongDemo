namespace Pong.AzureWebjob.Services;

public interface IPongService
{
    Task PongAsync(string message, CancellationToken cancellationToken = default);
}