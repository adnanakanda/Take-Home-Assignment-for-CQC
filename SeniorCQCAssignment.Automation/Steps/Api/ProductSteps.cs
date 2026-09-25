using SeniorCQCAssignment.Automation.ApiClients;
using SeniorCQCAssignment.Automation.Exceptions;
using SeniorCQCAssignment.Automation.Models.Api.Responses;

namespace SeniorCQCAssignment.Automation.Steps.Api;

public sealed class ProductSteps
{
    private readonly ProductsClient _productsClient;

    public ProductSteps(ProductsClient productsClient)
    {
        _productsClient = productsClient;
    }

    public async Task<IReadOnlyList<ProductResponse>> SearchProductAsync(string productname)
    {
        var response = await _productsClient.SearchAsync(productname);

        if (!response.IsSuccessStatusCode)
        {
            throw new ApiException($"Product search failed for query '{productname}': {response.ResponseBody}");
        }

        return response.Data?.Data ?? throw new ApiException($"Product search returned no data for query '{productname}'.");
    }

    public async Task<ProductResponse> FindProductAsync(string productName)
    {
        var products = await SearchProductAsync(productName);

        return products.FirstOrDefault(product => product.Name.Contains(productName, StringComparison.OrdinalIgnoreCase))
            ?? throw new ApiException($"Product '{productName}' was not found.");
    }
}