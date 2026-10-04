using OpenQA.Selenium;
using SeniorCQCAssignment.Automation.Components;
using SeniorCQCAssignment.Automation.Pages;
using SeniorCQCAssignment.Automation.Steps.Ui;
using SeniorCQCAssignment.Framework.WebDriver;

namespace SeniorCQCAssignment.Tests.Fixtures;

public abstract class UiTestBase : TestFixtureBase
{
    protected IWebDriver Driver { get; private set; } = null!;

    protected TimeSpan DefaultWait => Configuration.ExplicitWait;

    protected NavigationBar Header { get; private set; } = null!;

    protected LoginSteps LoginSteps { get; private set; } = null!;

    protected RegistrationSteps RegistrationSteps { get; private set; } = null!;

    protected SearchSteps SearchSteps { get; private set; } = null!;

    protected BasketUiSteps BasketUiSteps { get; private set; } = null!;

    [SetUp]
    public void SetUpUi()
    {
        Driver = WebDriverFactory.Create(Configuration, Logger);

        var searchPage = new SearchPage(Driver, Configuration, Logger);
        var homePage = new HomePage(Driver, Configuration, Logger);

        Header = new NavigationBar(Driver, Configuration, Logger);

        LoginSteps = new LoginSteps(new LoginPage(Driver, Configuration, Logger), Header);

        RegistrationSteps = new RegistrationSteps(new RegistrationPage(Driver, Configuration, Logger));

        SearchSteps = new SearchSteps(searchPage);

        BasketUiSteps = new BasketUiSteps(searchPage, new BasketPage(Driver, Configuration, Logger), Header);

        homePage.GoTo();

        homePage.DismissWelcomeBanner();

        homePage.DismissCookieNoticeIfDisplayed();

        homePage.WaitUntilDisplayed();
    }

    [TearDown]
    public void TearDownUi()
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