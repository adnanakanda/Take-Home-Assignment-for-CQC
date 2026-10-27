using Microsoft.Extensions.Configuration;

namespace SeniorCQCAssignment.Framework.Configuration;

public static class ConfigurationProvider
{
    private const int DefaultExplicitWaitSeconds = 10;
    private const int DefaultHttpTimeoutSeconds = 60;

    public static TestConfiguration Load()
    {
        var environment = Environment.GetEnvironmentVariable("TestSettings__Environment") ?? "local";

        var configuration = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("Configuration/appsettings.json", optional: false)
            .AddJsonFile($"Configuration/appsettings.{environment}.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        var settings = configuration.GetSection("TestSettings");

        return new TestConfiguration
        {
            Environment = settings["Environment"] ?? environment,

            BaseUrl = ReadText(settings, "BaseUrl"),

            Browser = settings["Browser"] ?? "chrome",

            Headless = ReadFlag(settings, "Headless", false),

            ExplicitWaitSeconds = ReadSeconds(settings, "ExplicitWaitSeconds", DefaultExplicitWaitSeconds),

            HttpTimeoutSeconds = ReadSeconds(settings, "HttpTimeoutSeconds", DefaultHttpTimeoutSeconds)
        };
    }

    private static string ReadText(IConfigurationSection settings, string key) =>
        string.IsNullOrWhiteSpace(settings[key])
            ? throw new InvalidOperationException($"TestSettings:{key} is not configured.")
            : settings[key]!;

    private static bool ReadFlag(IConfigurationSection settings, string key, bool defaultValue)
    {
        var configured = settings[key];

        if (string.IsNullOrWhiteSpace(configured))
        {
            return defaultValue;
        }

        return bool.TryParse(configured, out var value)
            ? value
            : throw new InvalidOperationException($"TestSettings:{key} is not true or false: '{configured}'.");
    }

    private static int ReadSeconds(IConfigurationSection settings, string key, int defaultValue)
    {
        var configured = settings[key];

        if (string.IsNullOrWhiteSpace(configured))
        {
            return defaultValue;
        }

        return int.TryParse(configured, out var seconds) && seconds > 0
            ? seconds
            : throw new InvalidOperationException($"TestSettings:{key} is not a number of seconds greater than zero: '{configured}'.");
    }
}