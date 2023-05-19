using System.Reflection;
using Common.Webjob.Extensions;
using Common.Webjob.Helpers;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Pong.AzureWebjob.Configuration;

namespace Pong.AzureWebjob;

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
                services.AddSingleton<INameResolver, ConfigurationResolver>();
            });
}