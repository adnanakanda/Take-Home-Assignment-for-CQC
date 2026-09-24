namespace SeniorCQCAssignment.Automation.Context;

public sealed class AuthenticationContext
{
    public string Token { get; set; } = string.Empty;

    public int BasketId { get; set; }

    public string Email { get; set; } = string.Empty;
}