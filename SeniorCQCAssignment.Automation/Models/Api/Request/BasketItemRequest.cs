using System.Text.Json.Serialization;

namespace SeniorCQCAssignment.Automation.Models.Api.Requests;

public sealed class BasketItemRequest
{
    [JsonPropertyName("ProductId")]
    public int ProductId { get; init; }

    [JsonPropertyName("BasketId")]
    public int BasketId { get; init; }

    [JsonPropertyName("quantity")]
    public int Quantity { get; init; } = 1;
}