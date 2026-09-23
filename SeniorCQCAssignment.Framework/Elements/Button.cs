using OpenQA.Selenium;

namespace SeniorCQCAssignment.Framework.Elements;

public class Button : BaseElement
{
    public Button(
        IWebDriver driver,
        By locator,
        string name,
        TimeSpan timeout)
        : base(driver, locator, name, timeout)
    {
    }
}