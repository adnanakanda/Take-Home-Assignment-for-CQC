using OpenQA.Selenium;
using SeniorCQCAssignment.Automation.Locators;
using SeniorCQCAssignment.Automation.Models.Domain;
using SeniorCQCAssignment.Framework.Configuration;
using SeniorCQCAssignment.Framework.Elements;
using SeniorCQCAssignment.Framework.Logging;

namespace SeniorCQCAssignment.Automation.Pages;

public class LoginPage : BasePage
{
    private const string Route = "#/login";

    private readonly TextBox _emailInput;
    private readonly Label _errorMessage;
    private readonly Button _loginButton;
    private readonly PasswordTextBox _passwordInput;

    public LoginPage(IWebDriver driver, TestConfiguration configuration, ILogger logger) : base(driver, configuration, logger)
    {
        _emailInput = Elements.TextBox(LoginPageLocators.EmailInput, "Email text box");

        _passwordInput = Elements.PasswordTextBox(LoginPageLocators.PasswordInput, "Password text box");

        _loginButton = Elements.Button(LoginPageLocators.LoginButton, "Login button");

        _errorMessage = Elements.Label(LoginPageLocators.ErrorMessage, "Login error message");
    }

    public void GoTo() => Navigator.GoToPage(Route);
    public override void WaitUntilDisplayed() => Wait.WaitForVisible(LoginPageLocators.Form);

    public void Login(User user)
    {
        _emailInput.SetValue(user.Email);

        _passwordInput.SetValue(user.Password);

        _loginButton.Click();
    }

    public string GetErrorMessage()
    {
        var message = _errorMessage.Text;

        Logger.Information($"The login error message is: '{message}'.");

        return message;
    }
}