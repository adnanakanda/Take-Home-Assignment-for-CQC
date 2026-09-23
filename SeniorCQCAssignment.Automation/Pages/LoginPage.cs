using OpenQA.Selenium;
using SeniorCQCAssignment.Framework.Elements;
using SeniorCQCAssignmnet.Tests.AQFramework.Models;

namespace SeniorCQCAssignment.Automation.Pages;

public class LoginPage
{
    private readonly TextBox _emailInput;
    private readonly TextBox _passwordInput;
    private readonly Button _loginButton;

    public LoginPage(
        IWebDriver driver,
        TimeSpan timeout)
    {
        _emailInput = new TextBox(driver, By.Id("email"), "Email text box", timeout);

        _passwordInput = new TextBox(driver, By.Id("password"), "Password text box", timeout);

        _loginButton = new Button(driver, By.Id("login"), "Login button", timeout);
    }

    public void Login(User user)
    {
        _emailInput.SetValue(user.Email);
        _passwordInput.SetValue(user.Password);
        _loginButton.Click();
    }
}