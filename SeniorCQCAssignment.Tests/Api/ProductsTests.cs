using SeniorCQCAssignment.Automation.Steps.Api;
using SeniorCQCAssignment.Tests.Fixtures;
using SeniorCQCAssignment.Tests.TestData;

namespace SeniorCQCAssignment.Tests.Api;

public class ProductsTests : ApiTestBase
{
    [TestCaseSource(typeof(ProductSearchData), nameof(ProductSearchData.Queries))]
    public async Task Search_Product(ProductSearchCase productsData)
    {
        //Arrange
        var productSteps = new ProductSteps(ProductsClient);

        //Act
        var products = await productSteps.SearchProductAsync(productsData.Query);

        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(products, Is.Not.Empty, $"No products were returned for search query '{productsData.Query}'.");
            Assert.That(products.Any(product => product.Name.Contains(productsData.ExpectedProduct, StringComparison.OrdinalIgnoreCase)), Is.True, $"No returned product matched expected product '{productsData.ExpectedProduct}' for search query '{productsData.Query}'.");
        });
    }
}