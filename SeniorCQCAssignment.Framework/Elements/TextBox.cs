using OpenQA.Selenium;

namespace SeniorCQCAssignment.Framework.Elements;

public class TextBox : BaseElement
{
    public TextBox(
        IWebDriver driver,
        By locator,
        string name,
        TimeSpan timeout)
        : base(driver, locator, name, timeout)
    {
    }

    public void SetValue(string value)
    {
        var element = ClickableElement;

        element.Clear();
        element.SendKeys(value);
    }

    public void Clear() => ClickableElement.Clear();

    public string GetValue() =>GetDomProperty("value");
}