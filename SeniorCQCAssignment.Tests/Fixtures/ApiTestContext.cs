using SeniorCQCAssignment.Automation.ApiClients;
using SeniorCQCAssignment.Automation.Context;
using SeniorCQCAssignment.Automation.Steps.Api;
using SeniorCQCAssignment.Framework.Configuration;
using SeniorCQCAssignment.Framework.HTTP;
using SeniorCQCAssignment.Framework.Logging;

namespace SeniorCQCAssignment.Tests.Fixtures;

public sealed class ApiTestContext : IDisposable
{
    public ApiTestContext(TestConfiguration configuration, ILogger logger)
    {
        ArgumentNullException.ThrowIfNull(configuration);
        ArgumentNullException.ThrowIfNull(logger);

        AuthenticationContext = new AuthenticationContext();

        Client = HttpClientFactory.Create(
            configuration.BaseUrl,
            logger,
            configuration.HttpTimeout);

        AuthenticatedClient = HttpClientFactory.Create(
            configuration.BaseUrl,
            logger,
            configuration.HttpTimeout,
            new AuthenticationHandler(AuthenticationContext));

        UsersClient = new UsersClient(Client);
        AuthenticationClient = new AuthenticationClient(Client);
        ProductsClient = new ProductsClient(Client);
        BasketClient = new BasketClient(AuthenticatedClient);

        UserSteps = new UserSteps(UsersClient, AuthenticationClient, AuthenticationContext, logger);
        ProductSteps = new ProductSteps(ProductsClient, logger);
        BasketSteps = new BasketSteps(BasketClient, AuthenticationContext, logger);
    }

    public HttpClient Client { get; }

    public HttpClient AuthenticatedClient { get; }

    public UsersClient UsersClient { get; }

    public AuthenticationClient AuthenticationClient { get; }

    public ProductsClient ProductsClient { get; }

    public BasketClient BasketClient { get; }

    public AuthenticationContext AuthenticationContext { get; }

    public UserSteps UserSteps { get; }

    public ProductSteps ProductSteps { get; }

    public BasketSteps BasketSteps { get; }

    public void Dispose()
    {
        AuthenticatedClient.Dispose();

        Client.Dispose();
    }
}