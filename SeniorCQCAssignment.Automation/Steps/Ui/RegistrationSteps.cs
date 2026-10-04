using SeniorCQCAssignment.Automation.Models.Domain;
using SeniorCQCAssignment.Automation.Pages;

namespace SeniorCQCAssignment.Automation.Steps.Ui;

public class RegistrationSteps
{
    private readonly RegistrationPage _registrationPage;

    public RegistrationSteps(RegistrationPage registrationPage)
    {
        _registrationPage = registrationPage;
    }

    public void Register(User user)
    {
        _registrationPage.GoTo();

        _registrationPage.WaitUntilDisplayed();

        _registrationPage.Register(user);
    }
}
