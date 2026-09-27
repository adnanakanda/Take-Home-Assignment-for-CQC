using SeniorCQCAssignment.Automation.TestData;
using SeniorCQCAssignment.Tests.Constants;
using SeniorCQCAssignment.Tests.Fixtures;

namespace SeniorCQCAssignment.Tests.Api;

public class BasketTests : ApiTestBase
{
    [Test]
    [Category(Categories.Api)]
    [Category(Categories.Smoke)]
    public async Task Add_Product_To_Basket()
    {
        //Arrange
        var user = UserFactory.Create();
        var productName = "Apple";

        //Act
        await RegisterUserAsync(user);
        await LoginUserAsync(user);
        var product = await ProductSteps.FindProductAsync(productName);
        var basketItem = await BasketSteps.AddProductAsync(product.Id);

        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(basketItem, Is.Not.Null, $"Basket item was not returned after adding product '{productName}'.");
            Assert.That(basketItem!.ProductId, Is.EqualTo(product.Id), $"Expected basket item ProductId to be '{product.Id}', but was '{basketItem.ProductId}'.");
            Assert.That(basketItem.BasketId, Is.EqualTo(AuthenticationContext.BasketId), $"Expected basket item BasketId to be '{AuthenticationContext.BasketId}', but was '{basketItem.BasketId}'.");
            Assert.That(basketItem.Quantity, Is.EqualTo(1), $"Expected product quantity in basket to be 1, but was '{basketItem.Quantity}'.");
        });
    }
}