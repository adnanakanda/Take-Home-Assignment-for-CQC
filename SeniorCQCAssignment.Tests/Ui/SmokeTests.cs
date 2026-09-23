using SeniorCQCAssignment.Tests.Fixtures;

namespace SeniorCQCAssignment.Tests.Ui;

public class SmokeTests : UiTestBase
{
    [Test]
    public void JuiceShop_ShouldOpenSuccessfully()
    {
        Driver.Navigate().GoToUrl(Configuration.BaseUrl);

        Assert.That(Driver.Title, Is.Not.Empty);
    }
}