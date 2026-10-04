using OpenQA.Selenium;
using SeniorCQCAssignment.Framework.Configuration;
using SeniorCQCAssignment.Framework.Logging;

namespace SeniorCQCAssignment.Framework.Elements;

public sealed class ElementFactory
{
    private readonly IWebDriver _driver;
    private readonly ILogger _logger;
    private readonly TimeSpan _timeout;

    public ElementFactory(
        IWebDriver driver,
        TestConfiguration configuration,
        ILogger logger)
    {
        ArgumentNullException.ThrowIfNull(driver);
        ArgumentNullException.ThrowIfNull(configuration);
        ArgumentNullException.ThrowIfNull(logger);

        _driver = driver;
        _timeout = configuration.ExplicitWait;
        _logger = logger;
    }

    public TextBox TextBox(By locator, string name) =>
        new(_driver, locator, name, _timeout, _logger);

    public PasswordTextBox PasswordTextBox(By locator, string name) =>
        new(_driver, locator, name, _timeout, _logger);

    public Button Button(By locator, string name) =>
        new(_driver, locator, name, _timeout, _logger);

    public Label Label(By locator, string name) =>
        new(_driver, locator, name, _timeout, _logger);

    public Element Element(By locator, string name) =>
        new(_driver, locator, name, _timeout, _logger);

    public CheckBox CheckBox(By locator, string name) =>
        new(_driver, locator, name, _timeout, _logger);

    public Dropdown Dropdown(By locator, By optionsLocator, string name) =>
        new(_driver, locator, optionsLocator, name, _timeout, _logger);
}
