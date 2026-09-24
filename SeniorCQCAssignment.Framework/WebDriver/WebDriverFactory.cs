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
            logger.Information("Creating Chrome driver");

            var options = new ChromeOptions();

            if (configuration.Headless)
            {
                logger.Information("Running Chrome in headless mode");

                options.AddArgument("--headless=new");
            }

            return new ChromeDriver(options);
        }
    }
}