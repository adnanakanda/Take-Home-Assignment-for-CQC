using OpenQA.Selenium;
using SeniorCQCAssignment.Framework.Logging;
using SeniorCQCAssignment.Framework.Waits;

namespace SeniorCQCAssignment.Framework.Elements;

public abstract class BaseElement
{
    protected BaseElement(IWebDriver driver, By locator, string name, TimeSpan timeout, ILogger logger)
    {
        Driver = driver ?? throw new ArgumentNullException(nameof(driver));
        Locator = locator ?? throw new ArgumentNullException(nameof(locator));
        Logger = logger ?? throw new ArgumentNullException(nameof(logger));

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

    protected ILogger Logger { get; }

    protected virtual bool IsSecret => false;

    protected IWebElement VisibleElement => Wait.WaitForVisible(Locator);

    protected IWebElement ClickableElement => Wait.WaitForClickable(Locator);

    public virtual void Click()
    {
        Logger.Information($"Clicking '{Name}'.");

        ClickableElement.Click();
    }

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

    public virtual bool IsDisplayedNow()
    {
        var elements = Driver.FindElements(Locator);

        return elements.Count > 0 && elements[0].Displayed;
    }

    public virtual bool WaitUntilInvisible() => Wait.WaitForInvisible(Locator);

    public virtual IWebElement WaitForText(string expectedText) => Wait.WaitForText(Locator, expectedText);

    protected string ValueForLog(string value) => IsSecret ? "***" : value;
}