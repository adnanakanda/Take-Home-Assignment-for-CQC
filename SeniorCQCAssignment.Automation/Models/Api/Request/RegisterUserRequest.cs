using System.Text.Json.Serialization;

namespace SeniorCQCAssignment.Automation.Models.Api.Requests;

public sealed class RegisterUserRequest
{
    [JsonPropertyName("email")]
    public string Email { get; init; } = string.Empty;


    [JsonPropertyName("password")]
    public string Password { get; init; } = string.Empty;


    [JsonPropertyName("passwordRepeat")]
    public string PasswordRepeat { get; init; } = string.Empty;


    [JsonPropertyName("securityQuestion")]
    public SecurityQuestionRequest SecurityQuestion { get; init; } = null!;


    [JsonPropertyName("securityAnswer")]
    public string SecurityAnswer { get; init; } = string.Empty;
}

public sealed class SecurityQuestionRequest
{
    [JsonPropertyName("id")]
    public int Id { get; init; }
}