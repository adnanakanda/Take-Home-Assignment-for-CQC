using SeniorCQCAssignment.Automation.TestData;
using SeniorCQCAssignment.Tests.Constants;
using SeniorCQCAssignment.Tests.Fixtures;

namespace SeniorCQCAssignment.Tests.Api;

public class AuthenticationTests : ApiTestBase
{
    [Test]
    [Category(Categories.Api)]
    [Category(Categories.Smoke)]
    public async Task Login_User()
    {
        //Arrange
        var user = UserFactory.Create();

        //Act
        await RegisterUserAsync(user);
        var token = await LoginUserAsync(user);

        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(token, Is.Not.Empty, "Login succeeded but authentication token was not returned.");
            Assert.That(AuthenticationContext.Token, Is.EqualTo(token), "Authentication context token does not match the login token.");
            Assert.That(AuthenticationContext.Email, Is.EqualTo(user.Email), $"Authenticated email does not match registered email '{user.Email}'.");
            Assert.That(AuthenticationContext.BasketId, Is.GreaterThan(0), "Login succeeded but a valid basket ID was not returned.");
        });
    }
}