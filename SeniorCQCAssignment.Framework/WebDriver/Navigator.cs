using OpenQA.Selenium;
using SeniorCQCAssignment.Framework.Configuration;
using SeniorCQCAssignment.Framework.Logging;

namespace SeniorCQCAssignment.Framework.WebDriver;

/// <summary>
/// Moves the browser to a page of the application under test. A page says which page it is, by the
/// route the application calls it, and the address that route is reached at comes from the
/// configuration. That is what lets the same suite run against another host without a page knowing
/// anything about where the application lives.
/// </summary>
public sealed class Navigator
{
    private readonly TestConfiguration _configuration;
    private readonly IWebDriver _driver;
    private readonly ILogger _logger;

    public Navigator(IWebDriver driver, TestConfiguration configuration, ILogger logger)
    {
        ArgumentNullException.ThrowIfNull(driver);
        ArgumentNullException.ThrowIfNull(configuration);
        ArgumentNullException.ThrowIfNull(logger);

        _driver = driver;
        _configuration = configuration;
        _logger = logger;
    }

    public void GoToStartPage()
    {
        _logger.Information($"Navigating to the start page at {_configuration.BaseUrl}.");

        _driver.Navigate().GoToUrl(_configuration.BaseUrl);
    }

    public void GoToPage(string route)
    {
        _logger.Information($"Navigating to '{route}'.");

        _driver.Navigate().GoToUrl($"{_configuration.BaseUrl}/{route}");
    }
}
