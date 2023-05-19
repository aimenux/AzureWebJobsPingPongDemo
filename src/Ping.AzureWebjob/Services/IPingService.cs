namespace Ping.AzureWebjob.Services;

public interface IPingService
{
    Task PingAsync(CancellationToken cancellationToken = default);
}