using SeniorCQCAssignment.Automation.ApiClients;
using SeniorCQCAssignment.Automation.Context;
using SeniorCQCAssignment.Automation.Exceptions;
using SeniorCQCAssignment.Automation.Models.Api.Requests;
using SeniorCQCAssignment.Automation.Models.Api.Responses;

namespace SeniorCQCAssignment.Automation.Steps.Api;

public sealed class BasketSteps
{
    private readonly BasketClient _basketClient;
    private readonly AuthenticationContext _authenticationContext;

    public BasketSteps(BasketClient basketClient, AuthenticationContext authenticationContext)
    {
        _basketClient = basketClient;
        _authenticationContext = authenticationContext;
    }

    public async Task<BasketItemResponse> AddProductAsync(int productId)
    {
        if (_authenticationContext.BasketId <= 0)
        {
            throw new InvalidOperationException("Cannot add product because the authenticated basket ID is not available.");
        }

        var request = new BasketItemRequest
        {
            ProductId = productId,
            BasketId = _authenticationContext.BasketId,
            Quantity = 1
        };

        var response = await _basketClient.AddItemAsync(request);

        if (!response.IsSuccessStatusCode)
        {
            throw new ApiException($"Failed to add product '{productId}' to basket: {response.ResponseBody}");
        }

        return response.Data?.Data ?? throw new ApiException("Basket item was added but no basket item was returned.");
    }
}