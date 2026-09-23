namespace SeniorCQCAssignment.Automation.TestData
{
    public class TestDataGenerator
    {
        public static string UniqueEmail() => $"a1qa-{Guid.NewGuid():N}@example.com";
        public static string Password() => $"A1qa!{Guid.NewGuid():N}";
    }
}
