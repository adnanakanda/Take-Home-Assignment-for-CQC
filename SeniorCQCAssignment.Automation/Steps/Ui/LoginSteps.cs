using SeniorCQCAssignment.Automation.Models.Domain;
using SeniorCQCAssignment.Automation.Pages;

public class LoginSteps
{
    private readonly LoginPage _loginPage;

    public LoginSteps(LoginPage loginPage)
    {
        _loginPage = loginPage;
    }

    public void Login(User user)
    {
        _loginPage.Login(user);
    }
}
