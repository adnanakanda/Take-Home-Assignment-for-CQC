using Bogus;
using SeniorCQCAssignment.Automation.Models.Domain;

namespace SeniorCQCAssignment.Automation.TestData;

public static class UserFactory
{
    private static readonly Faker<User> UserFaker =
        new Faker<User>()
            .CustomInstantiator(faker =>
                new User(
                    Email: faker.Internet.Email(),
                    Password: GeneratePassword(),
                    SecurityQuestion: "Your favorite book?",
                    SecurityAnswer: faker.Lorem.Sentence(5)
                ));

    public static User Create() => UserFaker.Generate();


    private static string GeneratePassword() => $"A1qa!{FakerProvider.Faker.Random.AlphaNumeric(12)}";
}