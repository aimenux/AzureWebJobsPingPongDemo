using Microsoft.Extensions.Configuration;

namespace Common.Webjob.Extensions;

public static class ConfigurationExtensions
{
    public static void AddJsonFile(this IConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.SetBasePath(PathExtensions.GetDirectoryPath());
        var environment = Environment.GetEnvironmentVariable("ENVIRONMENT");
        configurationBuilder.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
        configurationBuilder.AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true);
    }

    public static void AddAzureWebJobsStorage(this IConfigurationBuilder configurationBuilder)
    {
        var configuration = configurationBuilder.Build();
        configurationBuilder.AddInMemoryCollection(new Dictionary<string, string>
        {
            ["AzureWebJobsStorage"] = configuration.GetValue<string>("Settings:AzureWebJobsStorage")
        });
    }
}