using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using SeniorCQCAssignment.Framework.Configuration;
using SeniorCQCAssignment.Framework.Logging;

namespace SeniorCQCAssignment.Framework.WebDriver
{
    public static class WebDriverFactory
    {
        public static IWebDriver Create(TestConfiguration configuration, ILogger logger)
        {
            return configuration.Browser.ToLowerInvariant() switch
            {
                "chrome" => CreateChromeDriver(configuration, logger),
                _ => throw new NotSupportedException(
                    $"Browser '{configuration.Browser}' is not supported.")
            };
        }

        private static IWebDriver CreateChromeDriver(TestConfiguration configuration, ILogger logger)
        {
            var options = new ChromeOptions();

            options.AddArgument("--window-size=1920,1080");

            options.AddArgument("--no-sandbox");
            options.AddArgument("--disable-dev-shm-usage");

            if (configuration.Headless)
            {
                logger.Information("Running Chrome in headless mode");

                options.AddArgument("--headless=new");
            }

            return new ChromeDriver(options);
        }
    }
}