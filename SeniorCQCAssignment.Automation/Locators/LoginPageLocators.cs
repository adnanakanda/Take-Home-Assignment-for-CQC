using OpenQA.Selenium;

namespace SeniorCQCAssignment.Automation.Locators;

public static class LoginPageLocators
{
    public static readonly By Form = By.Id("login-form");

    public static readonly By EmailInput = By.Id("email");

    public static readonly By PasswordInput = By.Id("password");

    public static readonly By LoginButton = By.Id("loginButton");

    public static readonly By ErrorMessage = By.CssSelector("mat-card div.error");
}
