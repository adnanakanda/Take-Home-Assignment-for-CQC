using SeniorCQCAssignment.Automation.ApiClients;
using SeniorCQCAssignment.Automation.Exceptions;
using SeniorCQCAssignment.Automation.Models.Api.Responses;
using SeniorCQCAssignment.Framework.Logging;

namespace SeniorCQCAssignment.Automation.Steps.Api;

public sealed class ProductSteps
{
    private readonly ProductsClient _productsClient;
    private readonly ILogger _logger;

    public ProductSteps(ProductsClient productsClient, ILogger logger)
    {
        _productsClient = productsClient;
        _logger = logger;
    }

    public async Task<IReadOnlyList<ProductResponse>> SearchProductAsync(string productName)
    {
        _logger.Information($"Searching products for '{productName}' over the API.");

        var response = await _productsClient.SearchAsync(productName);

        if (!response.IsSuccessStatusCode)
        {
            throw new ApiException($"Product search failed for query '{productName}': {response.ResponseBody}");
        }

        var products = response.Data?.Data ?? throw new ApiException($"Product search returned no data for query '{productName}'.");

        _logger.Information($"The search for '{productName}' returned {products.Count} product(s): {string.Join(", ", products.Select(product => product.Name))}.");

        return products;
    }

    public async Task<ProductResponse> FindProductAsync(string productName)
    {
        var products = await SearchProductAsync(productName);

        var product = products.FirstOrDefault(product => product.Name.Contains(productName, StringComparison.OrdinalIgnoreCase))
            ?? throw new ApiException($"Product '{productName}' was not found.");

        _logger.Information($"Product '{product.Name}' is the one the search was after.");

        return product;
    }
}