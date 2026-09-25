using SeniorCQCAssignment.Automation.Steps.Api;
using SeniorCQCAssignment.Automation.TestData;
using SeniorCQCAssignment.Tests.Fixtures;

namespace SeniorCQCAssignment.Tests.Api;

public class BasketTests : ApiTestBase
{
    [Test]
    public async Task Add_Product_To_Basket()
    {
        //Arrange
        var user = UserFactory.Create();

        var userSteps = new UserSteps(UsersClient, AuthenticationClient, AuthenticationContext);
        var productSteps = new ProductSteps(ProductsClient);
        var basketSteps = new BasketSteps(BasketClient, AuthenticationContext);
        var productName = "Apple";

        //Act
        await userSteps.RegisterUserAsync(user);
        await userSteps.LoginUserAsync(user);
        var product = await productSteps.FindProductAsync(productName);
        var basketItem = await basketSteps.AddProductAsync(product.Id);

        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(basketItem, Is.Not.Null, $"Basket item was not returned after adding product '{productName}'.");
            Assert.That(basketItem!.ProductId, Is.EqualTo(product.Id), $"Expected basket item ProductId to be '{product.Id}', but was '{basketItem.ProductId}'.");
            Assert.That(basketItem.BasketId, Is.EqualTo(AuthenticationContext.BasketId), $"Expected basket item BasketId to be '{AuthenticationContext.BasketId}', but was '{basketItem.BasketId}'.");
            Assert.That(basketItem.Quantity, Is.EqualTo(1), $"Expected product quantity in basket to be 1, but was '{basketItem.Quantity}'."); // wiil remove magic numbers later
        });
    }
}