using OpenQA.Selenium;
using SeniorCQCAssignment.Framework.Configuration;
using SeniorCQCAssignment.Framework.Logging;
using SeniorCQCAssignment.Framework.WebDriver;
using ILogger = SeniorCQCAssignment.Framework.Logging.ILogger;

namespace SeniorCQCAssignment.Tests.Fixtures;

public abstract class UiTestBase
{
    protected IWebDriver Driver { get; private set; } = null!;
    protected ILogger Logger { get; private set; } = null!;

    protected TestConfiguration Configuration { get; private set; } = null!;

    protected TimeSpan DefaultWait => Configuration.ExplicitWait;

    [SetUp]
    public void BaseSetUp()
    {
        Logger = LoggerFactory.Create();

        Configuration = ConfigurationProvider.Load();

        Driver = WebDriverFactory.Create(Configuration, Logger);
    }

    [TearDown]
    public void BaseTearDown()
    {
        try
        {
            Driver.Quit();
        }
        finally
        {
            Driver.Dispose();
        }
    }
}