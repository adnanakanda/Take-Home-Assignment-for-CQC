using OpenQA.Selenium;

namespace SeniorCQCAssignment.Automation.Locators;

public static class BasketPageLocators
{
    public static readonly By ProductColumnCells = By.CssSelector("mat-table mat-cell.cdk-column-product");

    public static readonly By TotalPrice = By.Id("price");

    public static readonly By CheckoutButton = By.Id("checkoutButton");
}
