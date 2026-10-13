using SeniorCQCAssignment.Framework.HTTP;
using SeniorCQCAssignment.Tests.Constants;
using SeniorCQCAssignment.Tests.Fixtures;
using SeniorCQCAssignment.Tests.TestData;

namespace SeniorCQCAssignment.Tests.Api;

public class ProductsTests : ApiTestBase
{
    private const string SearchSchema = "product-search-response.schema.json";

    [TestCaseSource(typeof(ProductSearchData), nameof(ProductSearchData.Queries))]
    [Category(Categories.Api)]
    [Category(Categories.Smoke)]
    public async Task Search_Product(ProductSearchCase productsData)
    {
        //Act
        var products = await ProductSteps.SearchProductAsync(productsData.Query);

        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(products, Is.Not.Empty, $"No products were returned for search query '{productsData.Query}'.");
            Assert.That(products.Any(product => product.Name.Contains(productsData.ExpectedProduct, StringComparison.OrdinalIgnoreCase)), Is.True, $"No returned product matched expected product '{productsData.ExpectedProduct}' for search query '{productsData.Query}'.");
        });
    }

    [Test]
    [Category(Categories.Api)]
    [Category(Categories.Regression)]
    public async Task Search_Response_Matches_The_Schema()
    {
        //Arrange
        var schemaPath = Path.Combine(AppContext.BaseDirectory, "Schemas", SearchSchema);
        var schema = File.ReadAllText(schemaPath);

        //Act
        var response = await ProductSteps.SearchProductResponseAsync("Apple");
        var mismatches = JsonSchemaValidator.GetValidationErrors(response.ResponseBody, schema);

        //Assert
        Assert.That(mismatches, Is.Empty,
            $"The product search answer did not fit '{SearchSchema}':{Environment.NewLine}{string.Join(Environment.NewLine, mismatches)}");
    }
}