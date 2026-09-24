using SeniorCQCAssignment.Automation.Steps;
using SeniorCQCAssignment.Automation.TestData;
using SeniorCQCAssignment.Tests.Fixtures;

namespace SeniorCQCAssignment.Tests.Api;

public class AuthenticationTests : ApiTestBase
{
    [Test]
    public async Task User_Should_Register_And_Login()
    {
        var user = UserFactory.Create();
        var userSteps = new UserSteps(UsersClient, AuthenticationClient);
        await userSteps.RegisterUserAsync(user);
        var token = await userSteps.LoginUserAsync(user);
        Assert.That(token, Is.Not.Empty);
    }
}