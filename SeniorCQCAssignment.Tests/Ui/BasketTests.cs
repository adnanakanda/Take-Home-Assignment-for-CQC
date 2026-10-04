using SeniorCQCAssignment.Automation.TestData;
using SeniorCQCAssignment.Tests.Constants;
using SeniorCQCAssignment.Tests.Fixtures;

namespace SeniorCQCAssignment.Tests.Ui;

public class BasketTests : UiTestBase
{
    private const string ProductName = "Apple Juice (1000ml)";

    [Test]
    [Category(Categories.Ui)]
    [Category(Categories.Smoke)]
    public void Add_Product_To_The_Basket_From_The_Search_Results()
    {
        //Arrange
        var user = UserFactory.Create();

        RegistrationSteps.Register(user);

        LoginSteps.Login(user);

        SearchSteps.SearchFor(ProductName);

        //Act
        BasketUiSteps.AddProductToBasket(ProductName, 1);

        var basketProductNames = BasketUiSteps.OpenBasketAndGetProductNames();

        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(Header.GetBasketItemCount(), Is.EqualTo(1), "Expected the header to count one product in the basket.");

            Assert.That(basketProductNames, Does.Contain(ProductName), $"Expected the basket of '{user.Email}' to hold '{ProductName}', " +
                $"but it held: {string.Join(", ", basketProductNames)}.");

            Assert.That(BasketUiSteps.GetTotalPrice(), Is.Not.Empty, "Expected the basket to show a total price for the product in it.");
        });
    }
}