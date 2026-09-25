using SeniorCQCAssignment.Automation.Models.Api.Requests;
using SeniorCQCAssignment.Automation.Models.Domain;
using SeniorCQCAssignment.Automation.Steps.Api;
using SeniorCQCAssignment.Automation.TestData;
using SeniorCQCAssignment.Tests.Fixtures;
using SeniorCQCAssignment.Tests.TestData;
using System.Net;

namespace SeniorCQCAssignment.Tests.Api;

public class RegistrationTests : ApiTestBase
{
    [Test]
    public async Task Register_User()
    {
        //Arrange
        var user = UserFactory.Create();
        var userSteps = new UserSteps(UsersClient, AuthenticationClient, AuthenticationContext);

        //Act
        var response = await userSteps.RegisterUserAsync(user);
        TrackCreatedUser(response.Data!.Data!.Id);
        await userSteps.LoginUserAsync(user);

        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(response.IsSuccessStatusCode, Is.True, $"User registration failed for email '{user.Email}'.");
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created), $"Expected user registration to return 201 but received {(int)response.StatusCode}.");
        });
    }

    [TestCaseSource(typeof(RegistrationData), nameof(RegistrationData.InvalidCases))]
    public async Task Register_User_With_Invalid_Data(RegistrationCase registrationCase)
    {
        //Arrange
        var user = UserFactory.Create();
        var password = string.IsNullOrWhiteSpace(registrationCase.Password) ? user.Password : registrationCase.Password;
        var passwordRepeat = string.IsNullOrWhiteSpace(registrationCase.PasswordRepeat) ? user.Password : registrationCase.PasswordRepeat;
        var email = string.IsNullOrWhiteSpace(registrationCase.Email) ? user.Email : registrationCase.Email;

        if (registrationCase.DuplicateEmail)
        {
            email = user.Email;
        }

        var request = new RegisterUserRequest
        {
            Email = email,
            Password = password,
            PasswordRepeat = passwordRepeat,
            SecurityQuestion = new SecurityQuestionRequest { Id = user.SecurityQuestionId },
            SecurityAnswer = user.SecurityAnswer
        };

        //Act
        if (registrationCase.DuplicateEmail)
        {
            var userSteps = new UserSteps(UsersClient, AuthenticationClient, AuthenticationContext);
            var registrationResponse = await userSteps.RegisterUserAsync(user);
            TrackCreatedUser(registrationResponse.Data!.Data!.Id);
        }

        var response = await UsersClient.RegisterAsync(request);

        if (response.IsSuccessStatusCode && response.Data?.Data is not null)
        {
            TrackCreatedUser(response.Data.Data.Id);

            var createdUser = new User(
                email,
                password,
                user.SecurityQuestionId,
                user.SecurityAnswer);

            var userSteps = new UserSteps(
                UsersClient,
                AuthenticationClient,
                AuthenticationContext);

            await userSteps.LoginUserAsync(createdUser);
        }

        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(response.IsSuccessStatusCode, Is.False, $"Registration unexpectedly succeeded for '{registrationCase.Name}'.");
            Assert.That(response.StatusCode, Is.EqualTo((HttpStatusCode)registrationCase.ExpectedStatusCode), $"Expected '{registrationCase.Name}' registration to return {registrationCase.ExpectedStatusCode} but received {(int)response.StatusCode}.");
        });
    }
}