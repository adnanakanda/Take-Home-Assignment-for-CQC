using SeniorCQCAssignment.Tests.Constants;
using SeniorCQCAssignment.Tests.Fixtures;

namespace SeniorCQCAssignment.Tests.Ui;

public class SearchTests : UiTestBase
{
    private const string Query = "Apple";

    private const string ExpectedProduct = "Apple Juice";

    [Test]
    [Category(Categories.Ui)]
    [Category(Categories.Smoke)]
    public void Search_Product_Finds_The_Expected_Product()
    {

        //Act
        var productNames = SearchSteps.SearchFor(Query);

        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(productNames, Is.Not.Empty, $"Expected the search for '{Query}' to show at least one product, but it showed none.");

            Assert.That(
                productNames.Any(product => product.Contains(ExpectedProduct, StringComparison.OrdinalIgnoreCase)),
                Is.True,
                $"Expected a product called '{ExpectedProduct}' among the results for '{Query}', " +
                $"but found: {string.Join(", ", productNames)}.");
        });
    }
}