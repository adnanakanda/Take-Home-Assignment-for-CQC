using SeniorCQCAssignment.Automation.Pages;
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
        var homePage = new HomePage(Driver, Configuration, Logger);

        homePage.WaitUntilDisplayed();

        //Assert
        Assert.That(Driver.Title, Is.EqualTo("OWASP Juice Shop"), "Expected the title of the home page to be 'OWASP Juice Shop'.");
    }
}