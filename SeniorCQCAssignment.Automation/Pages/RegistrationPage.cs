using OpenQA.Selenium;
using SeniorCQCAssignment.Automation.Locators;
using SeniorCQCAssignment.Automation.Models.Domain;
using SeniorCQCAssignment.Framework.Configuration;
using SeniorCQCAssignment.Framework.Elements;
using SeniorCQCAssignment.Framework.Logging;

namespace SeniorCQCAssignment.Automation.Pages;

public class RegistrationPage : BasePage
{
    private const string Route = "#/register";

    private readonly TextBox _emailInput;
    private readonly TextBox _passwordInput;
    private readonly PasswordTextBox _passwordRepeatInput;
    private readonly Button _registerButton;
    private readonly TextBox _securityAnswerInput;
    private readonly Dropdown _securityQuestion;

    public RegistrationPage(IWebDriver driver, TestConfiguration configuration, ILogger logger) : base(driver, configuration, logger)
    {
        _emailInput = Elements.TextBox(RegistrationPageLocators.EmailInput, "Email text box");

        _passwordInput = Elements.PasswordTextBox(RegistrationPageLocators.PasswordInput, "Password text box");

        _passwordRepeatInput = Elements.PasswordTextBox(RegistrationPageLocators.PasswordRepeatInput, "Password confirmation text box");

        _securityQuestion = Elements.Dropdown(
            RegistrationPageLocators.SecurityQuestionSelect,
            RegistrationPageLocators.SecurityQuestionOptions,
            "Security question selection list");

        _securityAnswerInput = Elements.TextBox(RegistrationPageLocators.SecurityAnswerInput, "Security answer text box");

        _registerButton = Elements.Button(RegistrationPageLocators.RegisterButton, "Register button");
    }

    public void GoTo() => Navigator.GoToPage(Route);

    public override void WaitUntilDisplayed() => Wait.WaitForVisible(RegistrationPageLocators.RegisterButton);

    public void Register(User user)
    {
        _emailInput.SetValue(user.Email);

        _passwordInput.SetValue(user.Password);

        _passwordRepeatInput.SetValue(user.Password);

        _securityQuestion.SelectOption(user.SecurityQuestionId);

        _securityAnswerInput.SetValue(user.SecurityAnswer);

        _registerButton.Click();

        Wait.WaitForUrlContaining("#/login");

        Logger.Information($"'{user.Email}' is registered.");
    }
}
