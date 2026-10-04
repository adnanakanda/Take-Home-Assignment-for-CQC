using OpenQA.Selenium;

namespace SeniorCQCAssignment.Automation.Locators;


public static class SearchPageLocators
{
    public static readonly By SearchBar = By.Id("searchQuery");

    public static readonly By SearchInput = By.CssSelector("#searchQuery input");

    public static readonly By ResultTiles = By.CssSelector("mat-grid-tile");

    public static readonly By ProductNames = By.CssSelector("mat-grid-tile div.item-name");

    public static By AddToBasketButtonFor(string productName) =>
        By.XPath($"//mat-card[.//div[contains(@class, 'item-name') and normalize-space()='{productName}']]" + "//button[@aria-label='Add to Basket']");
}
