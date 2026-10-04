using OpenQA.Selenium;
using SeniorCQCAssignment.Framework.Logging;

namespace SeniorCQCAssignment.Framework.Elements;


public class Dropdown : BaseElement
{
    private readonly By _optionsLocator;

    public Dropdown(
        IWebDriver driver,
        By locator,
        By optionsLocator,
        string name,
        TimeSpan timeout,
        ILogger logger)
        : base(driver, locator, name, timeout, logger)
    {
        _optionsLocator = optionsLocator ?? throw new ArgumentNullException(nameof(optionsLocator));
    }

    public void Open() => Click();

    public void SelectOption(int position)
    {
        if (position < 1)
        {
            throw new ArgumentOutOfRangeException(
                nameof(position),
                position,
                "An option position counts from one.");
        }

        Open();

        Logger.Information($"Selecting option {position} in '{Name}'.");

        Wait.WaitForCount(_optionsLocator, position)[position - 1].Click();
    }
}
