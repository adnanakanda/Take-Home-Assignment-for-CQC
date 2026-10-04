using OpenQA.Selenium;
using SeniorCQCAssignment.Framework.Logging;

namespace SeniorCQCAssignment.Framework.Elements;

public class Element : BaseElement
{
    public Element(
        IWebDriver driver,
        By locator,
        string name,
        TimeSpan timeout,
        ILogger logger)
        : base(driver, locator, name, timeout, logger)
    {
    }
}
