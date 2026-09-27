using OpenQA.Selenium;
using SeniorCQCAssignment.Framework.WebDriver;

namespace SeniorCQCAssignment.Tests.Fixtures;

public abstract class UiTestBase : TestFixtureBase
{
    protected IWebDriver Driver { get; private set; } = null!;

    protected TimeSpan DefaultWait => Configuration.ExplicitWait;

    [SetUp]
    public void SetUpDriver()
    {
        Driver = WebDriverFactory.Create(Configuration, Logger);
    }

    [TearDown]
    public void TearDownDriver()
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