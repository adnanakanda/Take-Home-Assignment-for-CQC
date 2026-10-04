using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using SeniorCQCAssignment.Automation.Locators;
using SeniorCQCAssignment.Framework.Configuration;
using SeniorCQCAssignment.Framework.Elements;
using SeniorCQCAssignment.Framework.Logging;

namespace SeniorCQCAssignment.Automation.Components;

public class NavigationBar
{
    private readonly Button _accountMenuButton;
    private readonly Button _basketButton;
    private readonly Element _basketItemCount;
    private readonly Button _logoutButton;
    private readonly IWebDriver _driver;
    private readonly ElementFactory _elements;
    private readonly ILogger _logger;

    public NavigationBar(IWebDriver driver, TestConfiguration configuration, ILogger logger)
    {
        _driver = driver;
        _logger = logger;

        _elements = new ElementFactory(driver, configuration, logger);

        _accountMenuButton = _elements.Button(NavigationBarLocators.AccountMenuButton, "Account menu button");

        _logoutButton = _elements.Button(NavigationBarLocators.LogoutButton, "Logout menu item");

        _basketButton = _elements.Button(NavigationBarLocators.BasketButton, "Basket button");

        _basketItemCount = _elements.Element(NavigationBarLocators.BasketItemCount, "Basket item count");
    }

    public void GoToBasket() => _basketButton.Click();

    public int GetBasketItemCount()
    {
        var text = _basketItemCount.GetText().Trim();

        var itemCount = int.TryParse(text, out var count)
            ? count
            : 0;

        _logger.Information($"The basket in the navigation bar counts {itemCount} product(s).");

        return itemCount;
    }

    public void WaitForBasketItemCount(int expectedItemCount) => _basketItemCount.WaitForText(expectedItemCount.ToString());

    public bool IsSignedIn()
    {
        _accountMenuButton.Click();

        var signedIn = _logoutButton.IsDisplayed();

        CloseAccountMenu();

        _logger.Information($"The account menu offers the logout option: {signedIn}.");

        return signedIn;
    }

    private void CloseAccountMenu()
    {
        new Actions(_driver).SendKeys(Keys.Escape).Perform();

        _logoutButton.WaitUntilInvisible();
    }
}
