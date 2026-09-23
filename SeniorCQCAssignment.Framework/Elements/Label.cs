using OpenQA.Selenium;
using SeniorCQCAssignment.Framework.Elements;

namespace SeniorCQCAssignmnet.Tests.AQFramework.Elements
{
    public class Label : BaseElement
    {
        public Label(IWebDriver driver, By locator, string name, TimeSpan timeout)
            : base(driver, locator, name, timeout)
        {
        }

        public string Text => GetText();
    }
}
