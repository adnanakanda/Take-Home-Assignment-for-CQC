using OpenQA.Selenium;
using SeniorCQCAssignment.Framework.Logging;

namespace SeniorCQCAssignment.Framework.Elements;

public class CheckBox : BaseElement
{
    public CheckBox(
        IWebDriver driver,
        By locator,
        string name,
        TimeSpan timeout,
        ILogger logger)
        : base(driver, locator, name, timeout, logger)
    {
    }

    public bool IsSelected() => VisibleElement.Selected;

    public void Check()
    {
        if (!IsSelected())
        {
            Click();
        }
    }

    public void Uncheck()
    {
        if (IsSelected())
        {
            Click();
        }
    }
}