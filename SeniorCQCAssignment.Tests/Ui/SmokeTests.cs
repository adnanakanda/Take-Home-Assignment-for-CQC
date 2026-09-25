using SeniorCQCAssignment.Tests.Fixtures;

namespace SeniorCQCAssignment.Tests.Ui;

public class SmokeTests : UiTestBase
{
    [Test]
    public void JuiceShop_OpensSuccessfully()
    {
        //Act
        Driver.Navigate().GoToUrl(Configuration.BaseUrl);

        //Assert
        Assert.That(Driver.Title, Is.Not.Empty, "Expected Juice Shop title not displayed.");
    }
}