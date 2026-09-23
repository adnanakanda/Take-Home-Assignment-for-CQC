using OpenQA.Selenium;

namespace SeniorCQCAssignment.Framework.Elements
{
    public class CheckBox : BaseElement
    {
        public CheckBox(
            IWebDriver driver,
            By locator,
            string name,
            TimeSpan timeout)
            : base(driver, locator, name, timeout)
        {
        }

        public bool IsSelected() => VisibleElement.Selected;

        public void Check()
        {
            if (!IsSelected())
            {
                Click();
            }
        }

        public void Uncheck()
        {
            if (IsSelected())
            {
                Click();
            }
        }
    }
}
