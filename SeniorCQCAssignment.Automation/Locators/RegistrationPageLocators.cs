using OpenQA.Selenium;

namespace SeniorCQCAssignment.Automation.Locators;

public static class RegistrationPageLocators
{
    public static readonly By EmailInput = By.Id("emailControl");

    public static readonly By PasswordInput = By.Id("passwordControl");

    public static readonly By PasswordRepeatInput = By.Id("repeatPasswordControl");

    public static readonly By SecurityQuestionSelect = By.CssSelector("mat-select[name='securityQuestion']");

    public static readonly By SecurityQuestionOptions = By.CssSelector("mat-option");

    public static readonly By SecurityAnswerInput = By.Id("securityAnswerControl");

    public static readonly By RegisterButton = By.Id("registerButton");
}
