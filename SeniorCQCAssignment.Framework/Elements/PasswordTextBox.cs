using OpenQA.Selenium;
using SeniorCQCAssignment.Framework.Logging;

namespace SeniorCQCAssignment.Framework.Elements;

public class PasswordTextBox : TextBox
{
    public PasswordTextBox(
        IWebDriver driver,
        By locator,
        string name,
        TimeSpan timeout,
        ILogger logger)
        : base(driver, locator, name, timeout, logger)
    {
    }

    protected override bool IsSecret => true;
}
