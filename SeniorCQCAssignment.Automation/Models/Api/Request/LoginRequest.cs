using System.Text.Json.Serialization;

namespace SeniorCQCAssignment.Automation.Models.Api.Requests;

public sealed class LoginRequest
{
    [JsonPropertyName("email")]
    public string Email { get; init; } = string.Empty;

    [JsonPropertyName("password")]
    public string Password { get; init; } = string.Empty;
}