using System.Text.Json.Serialization;

namespace SeniorCQCAssignment.Automation.Models.Api.Responses;

public sealed class BasketResponse
{
    [JsonPropertyName("data")]
    public BasketItemResponse? Data { get; init; }
}

public sealed class BasketItemResponse
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("ProductId")]
    public int ProductId { get; init; }

    [JsonPropertyName("BasketId")]
    public int BasketId { get; init; }

    [JsonPropertyName("quantity")]
    public int Quantity { get; init; }
}