using OpenQA.Selenium;
using SeniorCQCAssignment.Framework.Logging;

namespace SeniorCQCAssignment.Framework.Elements;

public class TextBox : BaseElement
{
    public TextBox(
        IWebDriver driver,
        By locator,
        string name,
        TimeSpan timeout,
        ILogger logger)
        : base(driver, locator, name, timeout, logger)
    {
    }

    public void SetValue(string value)
    {
        Logger.Information($"Setting '{Name}' to '{ValueForLog(value)}'.");

        var element = ClickableElement;

        element.Clear();
        element.SendKeys(value);
    }


    public void PressEnter() => ClickableElement.SendKeys(Keys.Enter);

}