using SeniorCQCAssignment.Automation.Components;
using SeniorCQCAssignment.Automation.Pages;

namespace SeniorCQCAssignment.Automation.Steps.Ui;

public class BasketUiSteps
{
    private readonly BasketPage _basketPage;
    private readonly NavigationBar _header;
    private readonly SearchPage _searchPage;

    public BasketUiSteps(SearchPage searchPage, BasketPage basketPage, NavigationBar header)
    {
        _searchPage = searchPage;
        _basketPage = basketPage;
        _header = header;
    }

    public void AddProductToBasket(string productName, int expectedItemCount)
    {
        _searchPage.AddToBasket(productName);

        _header.WaitForBasketItemCount(expectedItemCount);
    }

    public IReadOnlyList<string> OpenBasketAndGetProductNames()
    {
        _header.GoToBasket();

        return _basketPage.GetProductNames();
    }

    public string GetTotalPrice() => _basketPage.GetTotalPrice();
}
