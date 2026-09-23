using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace SeniorCQCAssignment.Framework.Waits;

public class WaitHelper
{
    private readonly IWebDriver _driver;
    private readonly WebDriverWait _wait;

    public WaitHelper(IWebDriver driver, TimeSpan timeout)
    {
        _driver = driver;
        _wait = new WebDriverWait(driver, timeout);
    }

    public IWebElement WaitForVisible(By locator)
    {
        return _wait.Until(driver =>
        {
            try
            {
                var element = driver.FindElement(locator);

                return element.Displayed
                    ? element
                    : null;
            }
            catch (NoSuchElementException)
            {
                return null;
            }
            catch (StaleElementReferenceException)
            {
                return null;
            }
        })!;
    }

    public IWebElement WaitForClickable(By locator)
    {
        return _wait.Until(driver =>
        {
            try
            {
                var element = driver.FindElement(locator);

                return element.Displayed && element.Enabled
                    ? element
                    : null;
            }
            catch (NoSuchElementException)
            {
                return null;
            }
            catch (StaleElementReferenceException)
            {
                return null;
            }
        })!;
    }

    public bool WaitForInvisible(By locator)
    {
        return _wait.Until(driver =>
        {
            try
            {
                return !driver.FindElement(locator).Displayed;
            }
            catch (NoSuchElementException)
            {
                return true;
            }
            catch (StaleElementReferenceException)
            {
                return true;
            }
        });
    }
}