using SeniorCQCAssignment.Automation.ApiClients;
using SeniorCQCAssignment.Automation.Context;
using SeniorCQCAssignment.Automation.Exceptions;
using SeniorCQCAssignment.Automation.Models.Api.Requests;
using SeniorCQCAssignment.Automation.Models.Api.Responses;
using SeniorCQCAssignment.Framework.Logging;

namespace SeniorCQCAssignment.Automation.Steps.Api;

public sealed class BasketSteps
{
    private readonly BasketClient _basketClient;
    private readonly AuthenticationContext _authenticationContext;
    private readonly ILogger _logger;

    public BasketSteps(BasketClient basketClient, AuthenticationContext authenticationContext, ILogger logger)
    {
        _basketClient = basketClient;
        _authenticationContext = authenticationContext;
        _logger = logger;
    }

    public async Task<BasketItemResponse> AddProductAsync(int productId)
    {
        if (_authenticationContext.BasketId <= 0)
        {
            throw new InvalidOperationException("Cannot add product because the authenticated basket ID is not available.");
        }

        _logger.Information($"Adding product {productId} to basket {_authenticationContext.BasketId} over the API.");

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

        var basketItem = response.Data?.Data ?? throw new ApiException("Basket item was added but no basket item was returned.");

        _logger.Information($"The API stored product {basketItem.ProductId} in basket {basketItem.BasketId}, quantity {basketItem.Quantity}.");

        return basketItem;
    }

    public async Task<BasketDetails> GetBasketAsync(int basketId)
    {
        _logger.Information($"Reading basket {basketId} over the API.");

        var response = await _basketClient.GetBasketAsync(basketId);

        if (!response.IsSuccessStatusCode)
        {
            throw new ApiException($"Failed to read basket {basketId}: {response.ResponseBody}");
        }

        var basket = response.Data?.Data ?? throw new ApiException($"Basket {basketId} was read but no basket was returned.");

        _logger.Information(
            $"The API shows {basket.Products.Count} product(s) in basket {basket.Id}: " +
            $"{string.Join(", ", basket.Products.Select(product => $"{product.Name} x{product.BasketItem?.Quantity}"))}.");

        return basket;
    }
}