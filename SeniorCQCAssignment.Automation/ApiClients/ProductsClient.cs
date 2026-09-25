using SeniorCQCAssignment.Automation.Models.Api.Responses;
using SeniorCQCAssignment.Framework.HTTP;

namespace SeniorCQCAssignment.Automation.ApiClients;

public sealed class ProductsClient : ApiClientBase
{
    private const string ProductsEndpoint = "/api/Products";
    private const string SearchEndpoint = "/rest/products/search";


    public ProductsClient(HttpClient client) : base(client)
    {
    }

    public async Task<ApiResponse<ProductsResponse>> GetProductsAsync(
        CancellationToken cancellationToken = default)
    {
        return await GetAsync<ProductsResponse>(ProductsEndpoint, cancellationToken);
    }

    public Task<ApiResponse<ProductsResponse>> SearchAsync(string query, CancellationToken cancellationToken = default)
    {
        var endpoint = $"{SearchEndpoint}?q={Uri.EscapeDataString(query)}";

        return GetAsync<ProductsResponse>(endpoint, cancellationToken);
    }
}