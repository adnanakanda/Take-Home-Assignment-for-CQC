using SeniorCQCAssignment.Automation.TestData;
using SeniorCQCAssignment.Tests.Constants;
using SeniorCQCAssignment.Tests.Fixtures;

namespace SeniorCQCAssignment.Tests.Ui;

public class LoginTests : UiTestBase
{
    private const string InvalidCredentialsMessage = "Invalid email or password.";

    private const string WrongPassword = "A1qa!WrongPassword123";

    [Test]
    [Category(Categories.Ui)]
    [Category(Categories.Smoke)]
    public void Login_With_Valid_Credentials_Signs_The_Customer_In()
    {
        //Arrange
        var user = UserFactory.Create();

        RegistrationSteps.Register(user);

        //Act
        LoginSteps.Login(user);

        //Assert
        Assert.That(LoginSteps.IsSignedIn(), Is.True, $"Expected the account menu to offer the logout option after signing in as '{user.Email}'.");
    }

    [Test]
    [Category(Categories.Ui)]
    [Category(Categories.Regression)]
    public void Login_With_Invalid_Credentials_Is_Refused()
    {
        //Arrange
        var user = UserFactory.Create();

        RegistrationSteps.Register(user);

        var userWithWrongPassword = user with { Password = WrongPassword };

        //Act
        LoginSteps.Login(userWithWrongPassword);

        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(LoginSteps.GetErrorMessage(), Is.EqualTo(InvalidCredentialsMessage), $"Expected the login form to refuse a wrong password with '{InvalidCredentialsMessage}'.");

            Assert.That(LoginSteps.IsSignedIn(), Is.False, $"Expected no customer to be signed in after a login with a wrong password for '{user.Email}'.");
        });
    }
}