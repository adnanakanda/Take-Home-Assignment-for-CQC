using OpenQA.Selenium;
using SeniorCQCAssignment.Automation.Locators;
using SeniorCQCAssignment.Framework.Configuration;
using SeniorCQCAssignment.Framework.Elements;
using SeniorCQCAssignment.Framework.Logging;

namespace SeniorCQCAssignment.Automation.Pages;

public class SearchPage : BasePage
{
    private readonly Element _searchBar;
    private readonly TextBox _searchInput;

    public SearchPage(IWebDriver driver, TestConfiguration configuration, ILogger logger) : base(driver, configuration, logger)
    {
        _searchBar = Elements.Element(SearchPageLocators.SearchBar, "Header search bar");

        _searchInput = Elements.TextBox(SearchPageLocators.SearchInput, "Search text box");
    }

    public override void WaitUntilDisplayed() => Wait.WaitForVisible(SearchPageLocators.SearchBar);

    public void SearchFor(string query)
    {
        _searchBar.Click();

        _searchInput.SetValue(query);

        _searchInput.PressEnter();

        Wait.WaitForUrlContaining("#/search?q=");

        Wait.WaitForVisible(SearchPageLocators.ResultTiles);
    }

    public IReadOnlyList<string> GetProductNames()
    {
        var names = Driver.FindElements(SearchPageLocators.ProductNames).Select(product => product.Text.Trim()).ToList();

        Logger.Information($"The search results hold {names.Count} product(s): {string.Join(", ", names)}.");

        return names;
    }

    public void AddToBasket(string productName)
    {
        Logger.Information($"Adding '{productName}' to the basket.");
        Elements.Button(SearchPageLocators.AddToBasketButtonFor(productName), $"Add to basket button of '{productName}'").Click();
    }
}
