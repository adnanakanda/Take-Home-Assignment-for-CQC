using SeniorCQCAssignment.Automation.Models.Api.Requests;
using SeniorCQCAssignment.Automation.Models.Api.Responses;
using SeniorCQCAssignment.Framework.HTTP;

namespace SeniorCQCAssignment.Automation.ApiClients;

public sealed class BasketClient : ApiClientBase
{
    private const string BasketEndpoint = "/api/BasketItems";


    public BasketClient(HttpClient client) : base(client)
    {
    }

    public Task<ApiResponse<BasketResponse>> AddItemAsync(
        BasketItemRequest request,
        CancellationToken cancellationToken = default)
        =>
        PostAsync<BasketItemRequest, BasketResponse>(BasketEndpoint, request, cancellationToken);
}