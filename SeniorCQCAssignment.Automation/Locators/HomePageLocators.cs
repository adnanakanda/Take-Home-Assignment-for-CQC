using OpenQA.Selenium;

namespace SeniorCQCAssignment.Automation.Locators;

public static class HomePageLocators
{
    public static readonly By ProductGrid = By.CssSelector("mat-grid-tile");

    public static readonly By WelcomeBannerDismissButton = By.CssSelector("button[aria-label='Close Welcome Banner']");

    public static readonly By CookieNoticeDismissButton = By.CssSelector("button[aria-label='dismiss cookie message'], .cc-window button.cc-dismiss");
}
