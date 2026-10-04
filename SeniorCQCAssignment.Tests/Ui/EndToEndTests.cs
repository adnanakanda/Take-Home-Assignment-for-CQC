using SeniorCQCAssignment.Automation.TestData;
using SeniorCQCAssignment.Tests.Constants;
using SeniorCQCAssignment.Tests.Fixtures;

namespace SeniorCQCAssignment.Tests.Ui;


public class EndToEndTests : EndToEndTestBase
{
    private const string ProductName = "Apple Juice (1000ml)";

    [Test]
    [Category(Categories.Ui)]
    [Category(Categories.Api)]
    [Category(Categories.Smoke)]
    public async Task Api_Created_Customer_Fills_The_Basket_Through_The_Interface()
    {
        //Arrange
        var user = UserFactory.Create();

        var product = await Api.ProductSteps.FindProductAsync(ProductName);

        //Act
        await RegisterUserAsync(user);

        await LoginUserAsync(user);

        LoginSteps.Login(user);

        //Assert
        Assert.That(LoginSteps.IsSignedIn(), Is.True, $"Expected '{user.Email}' to be signed in after logging in through the interface.");

        //Act
        SearchSteps.SearchFor(ProductName);

        BasketUiSteps.AddProductToBasket(ProductName, expectedItemCount: 1);

        var basketProductNames = BasketUiSteps.OpenBasketAndGetProductNames();

        var basket = await Api.BasketSteps.GetBasketAsync(Api.AuthenticationContext.BasketId);

        var basketProducts = basket.Products;

        Assert.Multiple(() =>
        {

            Assert.That(basketProductNames, Does.Contain(ProductName), $"Expected the basket in the interface to hold '{ProductName}', " +
                $"but it held: {string.Join(", ", basketProductNames)}.");

            Assert.That(basketProducts.Any(stored => stored.Id == product.Id), Is.True,
                $"Expected product '{ProductName}' (id {product.Id}) in basket " +
                $"'{Api.AuthenticationContext.BasketId}' over the API, " +
                $"but the API returned: {string.Join(", ", basketProducts.Select(stored => stored.Name))}.");
        });
    }
}