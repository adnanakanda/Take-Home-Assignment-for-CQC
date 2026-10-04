using SeniorCQCAssignment.Automation.Components;
using SeniorCQCAssignment.Automation.Models.Domain;
using SeniorCQCAssignment.Automation.Pages;

namespace SeniorCQCAssignment.Automation.Steps.Ui;

public class LoginSteps
{
    private readonly NavigationBar _header;
    private readonly LoginPage _loginPage;

    public LoginSteps(LoginPage loginPage, NavigationBar header)
    {
        _loginPage = loginPage;
        _header = header;
    }

    public void Login(User user)
    {
        _loginPage.GoTo();

        _loginPage.WaitUntilDisplayed();

        _loginPage.Login(user);
    }

    public string GetErrorMessage() => _loginPage.GetErrorMessage();

    public bool IsSignedIn() => _header.IsSignedIn();
}
