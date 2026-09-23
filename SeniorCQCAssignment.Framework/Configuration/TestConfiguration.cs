namespace SeniorCQCAssignment.Framework.Configuration;

public class TestConfiguration
{
    public string Environment { get; init; } = "local";

    public string BaseUrl { get; init; } = string.Empty;

    public string Browser { get; init; } = "chrome";

    public bool Headless { get; init; }

    public int ExplicitWaitSeconds { get; init; } = 10;

    public int HttpTimeoutSeconds { get; init; } = 60;


    public TimeSpan ExplicitWait => TimeSpan.FromSeconds(ExplicitWaitSeconds);


    public TimeSpan HttpTimeout => TimeSpan.FromSeconds(HttpTimeoutSeconds);
}