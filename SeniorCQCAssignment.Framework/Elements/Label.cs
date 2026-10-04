using OpenQA.Selenium;
using SeniorCQCAssignment.Framework.Logging;

namespace SeniorCQCAssignment.Framework.Elements;

public class Label : BaseElement
{
    public Label(
        IWebDriver driver,
        By locator,
        string name,
        TimeSpan timeout,
        ILogger logger)
        : base(driver, locator, name, timeout, logger)
    {
    }

    public string Text => GetText();
}