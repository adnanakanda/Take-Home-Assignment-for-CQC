using OpenQA.Selenium;

namespace SeniorCQCAssignment.Automation.Locators;

public static class NavigationBarLocators
{
    public static readonly By AccountMenuButton = By.Id("navbarAccount");

    public static readonly By LogoutButton = By.CssSelector("button[aria-label='Logout']");

    public static readonly By BasketButton = By.CssSelector("button[aria-label='Show the shopping cart']");

    public static readonly By BasketItemCount = By.CssSelector("button[aria-label='Show the shopping cart'] .fa-layers-counter");
}
