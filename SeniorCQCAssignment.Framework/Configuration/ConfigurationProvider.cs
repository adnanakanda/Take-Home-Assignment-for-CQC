using Microsoft.Extensions.Configuration;

namespace SeniorCQCAssignment.Framework.Configuration;

public static class ConfigurationProvider
{
    public static TestConfiguration Load()
    {
        var environment = Environment.GetEnvironmentVariable("TestSettings__Environment") ?? "local";

        var configuration = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("Configuration/appsettings.json", optional: false)
            .AddJsonFile($"Configuration/appsettings.{environment}.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        var section = configuration.GetSection("TestSettings");

        return new TestConfiguration
        {
            Environment = section["Environment"] ?? environment,

            BaseUrl = section["BaseUrl"] ?? throw new InvalidOperationException("TestSettings:BaseUrl is not configured."),

            Browser = section["Browser"] ?? "chrome",

            Headless = bool.TryParse(section["Headless"], out var headless) && headless,

            ExplicitWaitSeconds = int.TryParse(section["ExplicitWaitSeconds"], out var waitSeconds) ? waitSeconds : 10
        };
    }
}