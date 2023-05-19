using System.Reflection;
using Azure.Storage.Queues;
using Common.Webjob.Extensions;
using Common.Webjob.Helpers;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Ping.AzureWebjob.Configuration;
using Ping.AzureWebjob.Services;

namespace Ping.AzureWebjob;

public static class Program
{
    private static readonly Assembly CurrentAssembly = typeof(Program).Assembly;
    
    public static async Task Main(string[] args)
    {
        using var host = CreateHostBuilder(args).Build();
        await host.RunAsync();
    }

    private static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebJobs(builder =>
            {
                builder.AddAzureStorageCoreServices();
                builder.UseHostId();
                builder.AddTimers();
            })
            .ConfigureAppConfiguration((_, config) =>
            {
                config.AddJsonFile();
                config.AddUserSecrets(CurrentAssembly);
                config.AddEnvironmentVariables();
                config.AddCommandLine(args);
                config.AddAzureWebJobsStorage();
            })
            .ConfigureLogging((hostingContext, loggingBuilder) =>
            {
                loggingBuilder.AddApplicationInsights(hostingContext.Configuration);
            })
            .ConfigureServices((hostingContext, services) =>
            {
                services.Configure<Settings>(hostingContext.Configuration.GetSection("Settings"));
                services.AddSingleton<IPingService, PingService>();
                services.AddSingleton<INameResolver, ConfigurationResolver>();
                services.AddTransient<IAzureQueueProvider, AzureQueueProvider>();
                services
                    .AddAzureClients(builder =>
                    {
                        builder.AddClient<QueueClient, QueueClientOptions>((_, _, serviceProvider) =>
                        {
                            var settings = serviceProvider.GetRequiredService<IOptions<Settings>>().Value;
                            var connectionString = settings.AzureWebJobsStorage;
                            var queueName = settings.AzureQueueName;
                            var options = new QueueClientOptions
                            {
                                MessageEncoding = QueueMessageEncoding.Base64
                            };
                            var client = new QueueClient(connectionString, queueName, options);
                            client.CreateIfNotExists();
                            return client;
                        });
                    });
            });
}