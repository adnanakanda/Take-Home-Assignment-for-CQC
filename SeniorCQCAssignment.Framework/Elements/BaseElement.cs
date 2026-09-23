using OpenQA.Selenium;
using SeniorCQCAssignment.Framework.Waits;

namespace SeniorCQCAssignment.Framework.Elements;

public abstract class BaseElement
{
    protected BaseElement(
        IWebDriver driver,
        By locator,
        string name,
        TimeSpan timeout)
    {
        Driver = driver ?? throw new ArgumentNullException(nameof(driver));
        Locator = locator ?? throw new ArgumentNullException(nameof(locator));

        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        if (timeout <= TimeSpan.Zero)
        {
            throw new ArgumentOutOfRangeException(
                nameof(timeout),
                timeout,
                "Timeout must be greater than zero.");
        }

        Name = name;
        Wait = new WaitHelper(driver, timeout);
    }

    protected IWebDriver Driver { get; }

    protected By Locator { get; }

    protected string Name { get; }

    protected WaitHelper Wait { get; }

    protected IWebElement VisibleElement => Wait.WaitForVisible(Locator);

    protected IWebElement ClickableElement => Wait.WaitForClickable(Locator);

    public virtual void Click() => ClickableElement.Click();

    public virtual string GetText() => VisibleElement.Text;

    public virtual string GetAttribute(string attributeName) => VisibleElement.GetAttribute(attributeName) ?? string.Empty;

    public virtual string GetDomAttribute(string attributeName) => VisibleElement.GetDomAttribute(attributeName) ?? string.Empty;

    public virtual string GetDomProperty(string propertyName) => VisibleElement.GetDomProperty(propertyName) ?? string.Empty;

    public virtual bool IsDisplayed()
    {
        try
        {
            return VisibleElement.Displayed;
        }
        catch (WebDriverTimeoutException)
        {
            return false;
        }
    }

    public virtual bool IsEnabled()
    {
        try
        {
            return VisibleElement.Enabled;
        }
        catch (WebDriverTimeoutException)
        {
            return false;
        }
    }
}