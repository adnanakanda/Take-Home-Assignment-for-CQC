using SeniorCQCAssignment.Automation.Context;
using SeniorCQCAssignment.Automation.Steps.Api;

namespace SeniorCQCAssignment.Tests.Fixtures;

public abstract class ApiTestBase : TestFixtureBase
{
    protected AuthenticationContext AuthenticationContext => Api.AuthenticationContext;

    protected ProductSteps ProductSteps => Api.ProductSteps;

    protected BasketSteps BasketSteps => Api.BasketSteps;
}
