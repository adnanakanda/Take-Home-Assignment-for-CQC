using Bogus;

namespace SeniorCQCAssignment.Automation.TestData;

public static class FakerProvider
{
    public static Faker Faker { get; } = new();
}