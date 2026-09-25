using System.Text.Json.Serialization;

namespace SeniorCQCAssignment.Automation.Models.Api.Responses;

public sealed class RegisterUserResponse
{
    [JsonPropertyName("status")]
    public string Status { get; init; } = string.Empty;

    [JsonPropertyName("data")]
    public RegisterUserData? Data { get; init; }
}

public sealed class RegisterUserData
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("email")]
    public string Email { get; init; } = string.Empty;

    [JsonPropertyName("username")]
    public string Username { get; init; } = string.Empty;

    [JsonPropertyName("role")]
    public string Role { get; init; } = string.Empty;
}