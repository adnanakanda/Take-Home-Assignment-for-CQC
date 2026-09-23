using OpenQA.Selenium;
using SeniorCQCAssignment.Framework.Configuration;
using SeniorCQCAssignment.Framework.WebDrivers;

namespace SeniorCQCAssignment.Tests.Fixtures;

public abstract class UiTestBase
{
    protected IWebDriver Driver { get; private set; } = null!;

    protected TestConfiguration Configuration { get; private set; } = null!;

    protected TimeSpan DefaultWait => TimeSpan.FromSeconds(Configuration.ExplicitWaitSeconds);

    [SetUp]
    public void BaseSetUp()
    {
        Configuration = ConfigurationProvider.Load();
        Driver = WebDriverFactory.Create(Configuration);
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