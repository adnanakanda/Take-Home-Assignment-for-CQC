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

    public IWebElement WaitForText(By locator, string expectedText)
    {
        return _wait.Until(driver =>
        {
            try
            {
                var element = driver.FindElement(locator);

                return element.Text.Trim().Equals(expectedText.Trim(), StringComparison.OrdinalIgnoreCase)
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

    public bool WaitForUrlContaining(string urlFragment)
    {
        return _wait.Until(driver => driver.Url.Contains(urlFragment, StringComparison.OrdinalIgnoreCase));
    }

    public IReadOnlyList<IWebElement> WaitForCount(By locator, int expectedCount)
    {
        if (expectedCount < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(expectedCount), expectedCount, "An element count cannot be less than one.");
        }

        return _wait.Until(driver =>
        {
            try
            {
                var elements = driver.FindElements(locator);

                return elements.Count >= expectedCount
                    ? elements
                    : null;
            }
            catch (StaleElementReferenceException)
            {
                return null;
            }
        })!;
    }
}