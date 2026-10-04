using System.Text.Json.Serialization;

namespace SeniorCQCAssignment.Automation.Models.Api.Responses;

public sealed class BasketDetailsResponse
{
    [JsonPropertyName("data")]
    public BasketDetails? Data { get; init; }
}

public sealed class BasketDetails
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("UserId")]
    public int UserId { get; init; }

    [JsonPropertyName("Products")]
    public List<BasketProduct> Products { get; init; } = [];
}

public sealed class BasketProduct
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("name")]
    public string Name { get; init; } = string.Empty;

    [JsonPropertyName("BasketItem")]
    public BasketItem? BasketItem { get; init; }
}

public sealed class BasketItem
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("quantity")]
    public int Quantity { get; init; }
}
