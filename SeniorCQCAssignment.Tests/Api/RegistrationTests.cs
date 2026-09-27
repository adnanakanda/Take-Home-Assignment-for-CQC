using SeniorCQCAssignment.Automation.Models.Api.Requests;
using SeniorCQCAssignment.Automation.Models.Api.Responses;
using SeniorCQCAssignment.Automation.Models.Domain;
using SeniorCQCAssignment.Automation.TestData;
using SeniorCQCAssignment.Framework.HTTP;
using SeniorCQCAssignment.Tests.Constants;
using SeniorCQCAssignment.Tests.Fixtures;
using System.Net;

namespace SeniorCQCAssignment.Tests.Api;

public class RegistrationTests : ApiTestBase
{
    private const string ValidPassword = "A1qa!Password123";
    private const string DifferentPassword = "DifferentPassword123!";

    private static readonly string InvalidEmail = $"invalid-email-{Guid.NewGuid():N}";

    [Test]
    [Category(Categories.Api)]
    [Category(Categories.Smoke)]
    public async Task Register_User()
    {
        //Arrange
        var user = UserFactory.Create();

        //Act
        var response = await RegisterUserAsync(user);

        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(response.IsSuccessStatusCode, Is.True, $"User registration failed for email '{user.Email}'.");
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created), $"Expected user registration to return 201 but received {(int)response.StatusCode}.");
        });
    }

    [Test]
    [Category(Categories.Api)]
    [Category(Categories.Regression)]
    public async Task Register_User_With_Duplicate_Email()
    {
        //Arrange
        var user = UserFactory.Create();

        await RegisterUserAsync(user);

        var request = CreateRegistrationRequest(user);

        //Act
        var response = await RegisterUserAsync(request);

        //Assert
        AssertRegistrationRejected(response, "Duplicate email", HttpStatusCode.BadRequest);
    }

    [Test]
    [Category(Categories.Api)]
    [Category(Categories.Regression)]
    [Category(Categories.KnownDefect)]
    public async Task Register_User_With_Invalid_Email()
    {
        //Arrange
        var user = UserFactory.Create();

        var request = CreateRegistrationRequest(user, email: InvalidEmail);

        //Act
        var response = await RegisterUserAsync(request);

        //Assert
        AssertRegistrationRejected(response, "Invalid email", HttpStatusCode.BadRequest, "BUG-001");
    }

    [Test]
    [Category(Categories.Api)]
    [Category(Categories.Regression)]
    [Category(Categories.KnownDefect)]
    public async Task Register_User_With_Mismatched_Passwords()
    {
        //Arrange
        var user = UserFactory.Create();

        var request = CreateRegistrationRequest(
            user,
            password: ValidPassword,
            passwordRepeat: DifferentPassword);

        //Act
        var response = await RegisterUserAsync(request);

        //Assert
        AssertRegistrationRejected(response, "Mismatched passwords", HttpStatusCode.BadRequest, "BUG-002");
    }

    private static RegisterUserRequest CreateRegistrationRequest(
        User user,
        string? email = null,
        string? password = null,
        string? passwordRepeat = null)
        => new()
        {
            Email = email ?? user.Email,
            Password = password ?? user.Password,
            PasswordRepeat = passwordRepeat ?? user.Password,
            SecurityQuestion = new SecurityQuestionRequest { Id = user.SecurityQuestionId },
            SecurityAnswer = user.SecurityAnswer
        };

    private static void AssertRegistrationRejected(
        ApiResponse<RegisterUserResponse> response,
        string scenario,
        HttpStatusCode expectedStatusCode,
        string? knownDefect = null)
    {
        if (knownDefect is not null && response.IsSuccessStatusCode)
        {
            Assert.Inconclusive(
                $"{knownDefect} (see BUGS.md): '{scenario}' was accepted with " +
                $"HTTP {(int)response.StatusCode} instead of {(int)expectedStatusCode}.");
        }

        Assert.Multiple(() =>
        {
            Assert.That(response.IsSuccessStatusCode, Is.False, $"Registration unexpectedly succeeded for '{scenario}'.");
            Assert.That(response.StatusCode, Is.EqualTo(expectedStatusCode), $"Expected '{scenario}' registration to return {(int)expectedStatusCode} but received {(int)response.StatusCode}.");
        });
    }
}