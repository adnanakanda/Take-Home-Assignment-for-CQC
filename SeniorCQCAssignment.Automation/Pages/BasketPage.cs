using OpenQA.Selenium;
using SeniorCQCAssignment.Automation.Locators;
using SeniorCQCAssignment.Framework.Configuration;
using SeniorCQCAssignment.Framework.Elements;
using SeniorCQCAssignment.Framework.Logging;

namespace SeniorCQCAssignment.Automation.Pages;

public class BasketPage : BasePage
{
    private readonly Label _totalPrice;

    public BasketPage(IWebDriver driver, TestConfiguration configuration, ILogger logger) : base(driver, configuration, logger)
    {
        _totalPrice = Elements.Label(BasketPageLocators.TotalPrice, "Total price");
    }

    public override void WaitUntilDisplayed() => Wait.WaitForVisible(BasketPageLocators.CheckoutButton);

    public IReadOnlyList<string> GetProductNames()
    {
        Wait.WaitForVisible(BasketPageLocators.ProductColumnCells);

        var names = Driver.FindElements(BasketPageLocators.ProductColumnCells).Select(cell => cell.Text.Trim()).ToList();

        Logger.Information($"The basket holds {names.Count} product(s): {string.Join(", ", names)}.");

        return names;
    }

    public string GetTotalPrice()
    {
        var total = _totalPrice.Text;

        Logger.Information($"The basket comes to {total}.");

        return total;
    }
}
