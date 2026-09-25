using System.Text.Json.Serialization;

namespace SeniorCQCAssignment.Automation.Models.Api.Responses;

public sealed class ProductsResponse
{
    [JsonPropertyName("data")]
    public List<ProductResponse> Data { get; init; } = [];
}

public sealed class ProductResponse
{
    [JsonPropertyName("id")]
    public int Id { get; init; }


    [JsonPropertyName("name")]
    public string Name { get; init; } = string.Empty;


    [JsonPropertyName("price")]
    public decimal Price { get; init; }
}