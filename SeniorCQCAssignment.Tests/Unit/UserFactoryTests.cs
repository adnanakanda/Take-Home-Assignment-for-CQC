using SeniorCQCAssignment.Automation.TestData;
using SeniorCQCAssignment.Tests.Constants;

namespace SeniorCQCAssignment.Tests.Unit;

public class UserFactoryTests
{
    [Test]
    [Category(Categories.Unit)]
    public void Create_ReturnsUserWithValidEmail()
    {
        var user = UserFactory.Create();

        var parts = user.Email.Split('@');

        Assert.Multiple(() =>
        {
            Assert.That(parts, Has.Length.EqualTo(2), $"'{user.Email}' is not an address with one at sign in it.");
            Assert.That(parts[0], Is.Not.Empty, $"'{user.Email}' has nothing in front of the at sign.");
            Assert.That(parts[1], Does.Contain("."), $"'{user.Email}' has no domain to deliver to.");
            Assert.That(user.Email, Does.Not.Contain(" "), $"'{user.Email}' holds a space.");
        });
    }

    [Test]
    [Category(Categories.Unit)]
    public void Create_ReturnsUserWithStrongPassword()
    {
        var user = UserFactory.Create();

        Assert.Multiple(() =>
        {
            Assert.That(user.Password, Has.Length.AtLeast(8), "A password shorter than eight characters is not a password a shop should be tested with.");
            Assert.That(user.Password.Any(char.IsUpper), Is.True, "The password has no capital letter.");
            Assert.That(user.Password.Any(char.IsLower), Is.True, "The password has no small letter.");
            Assert.That(user.Password.Any(char.IsDigit), Is.True, "The password has no digit.");
            Assert.That(user.Password.Any(character => !char.IsLetterOrDigit(character)), Is.True, "The password has no symbol.");
        });
    }

    [Test]
    [Category(Categories.Unit)]
    public void Create_ReturnsUserWithValidSecurityQuestion()
    {
        var user = UserFactory.Create();

        Assert.Multiple(() =>
        {
            Assert.That(user.SecurityQuestionId, Is.GreaterThan(0), "The security question is picked by position, so there is no question number zero.");
            Assert.That(user.SecurityAnswer, Is.Not.Empty, "The registration form asks for an answer to the security question.");
        });
    }
}