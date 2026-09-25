using System.Text.Json.Serialization;

namespace SeniorCQCAssignment.Automation.Models.Api.Responses;

public sealed class LoginResponse
{
    [JsonPropertyName("authentication")]
    public AuthenticationResponse? Authentication { get; init; }
}

public sealed class AuthenticationResponse
{
    [JsonPropertyName("token")]
    public string Token { get; init; } = string.Empty;

    [JsonPropertyName("bid")]
    public int BasketId { get; init; }

    [JsonPropertyName("umail")]
    public string Email { get; init; } = string.Empty;
}