using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using SeniorCQCAssignment.Framework.Configuration;

namespace SeniorCQCAssignment.Framework.WebDrivers
{
    public static class WebDriverFactory
    {
        public static IWebDriver Create(TestConfiguration configuration)
        {
            return configuration.Browser.ToLowerInvariant() switch
            {
                "chrome" => CreateChromeDriver(configuration),
                _ => throw new NotSupportedException(
                    $"Browser '{configuration.Browser}' is not supported.")
            };
        }

        private static IWebDriver CreateChromeDriver(TestConfiguration configuration)
        {
            var options = new ChromeOptions();

            if (configuration.Headless)
            {
                options.AddArgument("--headless=new");
            }

            options.AddArgument("--window-size=1920,1080");
            options.AddArgument("--disable-dev-shm-usage");
            options.AddArgument("--no-sandbox");

            return new ChromeDriver(options);
        }
    }
}
