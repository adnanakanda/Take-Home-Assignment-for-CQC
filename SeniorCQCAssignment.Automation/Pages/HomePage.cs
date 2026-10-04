using OpenQA.Selenium;
using SeniorCQCAssignment.Automation.Locators;
using SeniorCQCAssignment.Framework.Configuration;
using SeniorCQCAssignment.Framework.Elements;
using SeniorCQCAssignment.Framework.Logging;

namespace SeniorCQCAssignment.Automation.Pages;

public class HomePage : BasePage
{
    private readonly Element _cookieNoticeDismissButton;
    private readonly Element _welcomeBannerDismissButton;

    public HomePage(IWebDriver driver, TestConfiguration configuration, ILogger logger) : base(driver, configuration, logger)
    {
        _welcomeBannerDismissButton = Elements.Element(HomePageLocators.WelcomeBannerDismissButton, "Welcome banner dismiss button");

        _cookieNoticeDismissButton = Elements.Element(HomePageLocators.CookieNoticeDismissButton, "Cookie notice dismiss button");
    }

    public void GoTo() => Navigator.GoToStartPage();

    public override void WaitUntilDisplayed() => Wait.WaitForVisible(HomePageLocators.ProductGrid);


    public void DismissWelcomeBanner()
    {
        _welcomeBannerDismissButton.Click();

        _welcomeBannerDismissButton.WaitUntilInvisible();
    }

    public void DismissCookieNoticeIfDisplayed()
    {
        if (_cookieNoticeDismissButton.IsDisplayedNow())
        {
            _cookieNoticeDismissButton.Click();

            _cookieNoticeDismissButton.WaitUntilInvisible();
        }
    }
}
