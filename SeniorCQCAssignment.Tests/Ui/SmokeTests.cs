using SeniorCQCAssignment.Tests.Constants;
using SeniorCQCAssignment.Tests.Fixtures;

namespace SeniorCQCAssignment.Tests.Ui;

public class SmokeTests : UiTestBase
{
    [Test]
    [Category(Categories.Ui)]
    [Category(Categories.Smoke)]
    public void JuiceShop_OpensSuccessfully()
    {
        //Act
        Driver.Navigate().GoToUrl(Configuration.BaseUrl);

        //Assert
        Assert.That(Driver.Title, Is.Not.Empty, "Expected Juice Shop title not displayed.");
    }
}