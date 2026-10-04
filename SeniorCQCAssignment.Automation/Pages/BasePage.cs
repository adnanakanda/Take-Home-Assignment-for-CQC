using OpenQA.Selenium;
using SeniorCQCAssignment.Automation.Components;
using SeniorCQCAssignment.Framework.Configuration;
using SeniorCQCAssignment.Framework.Elements;
using SeniorCQCAssignment.Framework.Logging;
using SeniorCQCAssignment.Framework.WebDriver;
using SeniorCQCAssignment.Framework.Waits;

namespace SeniorCQCAssignment.Automation.Pages;

public abstract class BasePage
{
    protected BasePage(IWebDriver driver, TestConfiguration configuration, ILogger logger)
    {
        ArgumentNullException.ThrowIfNull(driver);
        ArgumentNullException.ThrowIfNull(configuration);
        ArgumentNullException.ThrowIfNull(logger);

        Driver = driver;
        Logger = logger;
        Timeout = configuration.ExplicitWait;

        Wait = new WaitHelper(driver, Timeout);
        Elements = new ElementFactory(driver, configuration, logger);
        Header = new NavigationBar(driver, configuration, logger);
        Navigator = new Navigator(driver, configuration, logger);
    }

    protected IWebDriver Driver { get; }

    protected TimeSpan Timeout { get; }

    protected WaitHelper Wait { get; }

    protected ILogger Logger { get; }

    protected ElementFactory Elements { get; }

    protected NavigationBar Header { get; }

    protected Navigator Navigator { get; }

    public abstract void WaitUntilDisplayed();
}
