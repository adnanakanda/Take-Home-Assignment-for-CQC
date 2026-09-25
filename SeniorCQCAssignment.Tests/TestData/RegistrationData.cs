using Bogus;

namespace SeniorCQCAssignment.Tests.TestData;

public sealed record RegistrationCase(string Name, string Email, string Password, string PasswordRepeat, bool DuplicateEmail, int ExpectedStatusCode);

public static class RegistrationData
{
    private static readonly Faker Faker = new();

    public static IEnumerable<RegistrationCase> InvalidCases =>
    [
        new("Duplicate email", string.Empty, string.Empty, string.Empty, true, 400),
        new("Invalid email", $"{Faker.Random.Word()}invalidEmail", "A1qa!Password123", "A1qa!Password123", false, 400),
        new("Mismatched passwords", string.Empty, "A1qa!Password123", "DifferentPassword123!", false, 400)
    ];
}