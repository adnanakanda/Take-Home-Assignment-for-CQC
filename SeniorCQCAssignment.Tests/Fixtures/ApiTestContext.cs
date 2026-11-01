using SeniorCQCAssignment.Automation.ApiClients;
using SeniorCQCAssignment.Automation.Context;
using SeniorCQCAssignment.Automation.Steps.Api;
using SeniorCQCAssignment.Framework.Configuration;
using SeniorCQCAssignment.Framework.HTTP;
using SeniorCQCAssignment.Framework.Logging;

namespace SeniorCQCAssignment.Tests.Fixtures;

public sealed class ApiTestContext : IDisposable
{
    private readonly string _contextToken;
    private ApiResponse<object>? _lastResponse;

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

        _contextToken = HttpContextStore.Register(this);

        Client.DefaultRequestHeaders.TryAddWithoutValidation("X-Test-Context", _contextToken);
        AuthenticatedClient.DefaultRequestHeaders.TryAddWithoutValidation("X-Test-Context", _contextToken);

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

    public ApiResponse<object>? LastResponse => _lastResponse;

    internal void RecordResponse<T>(ApiResponse<T> response)
    {
        _lastResponse = new ApiResponse<object>
        {
            StatusCode = response.StatusCode,
            RequestBody = response.RequestBody,
            ResponseBody = response.ResponseBody,
            Data = response.Data
        };
    }

    public void Dispose()
    {
        HttpContextStore.Unregister(_contextToken);

        AuthenticatedClient.Dispose();

        Client.Dispose();
    }
}
