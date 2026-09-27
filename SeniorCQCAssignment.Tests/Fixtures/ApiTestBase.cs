using SeniorCQCAssignment.Automation.ApiClients;
using SeniorCQCAssignment.Automation.Context;
using SeniorCQCAssignment.Automation.Models.Api.Requests;
using SeniorCQCAssignment.Automation.Models.Api.Responses;
using SeniorCQCAssignment.Automation.Models.Domain;
using SeniorCQCAssignment.Automation.Steps.Api;
using SeniorCQCAssignment.Framework.HTTP;

namespace SeniorCQCAssignment.Tests.Fixtures;

public abstract class ApiTestBase : TestFixtureBase
{
    protected HttpClient Client { get; private set; } = null!;
    protected HttpClient AuthenticatedClient { get; private set; } = null!;
    protected UsersClient UsersClient { get; private set; } = null!;
    protected AuthenticationClient AuthenticationClient { get; private set; } = null!;
    protected ProductsClient ProductsClient { get; private set; } = null!;
    protected BasketClient BasketClient { get; private set; } = null!;
    protected AuthenticationContext AuthenticationContext { get; private set; } = null!;

    protected UserSteps UserSteps { get; private set; } = null!;
    protected ProductSteps ProductSteps { get; private set; } = null!;
    protected BasketSteps BasketSteps { get; private set; } = null!;

    [SetUp]
    public void SetUpApiClients()
    {
        AuthenticationContext = new AuthenticationContext();

        Client = HttpClientFactory.Create(
            Configuration.BaseUrl,
            Logger,
            Configuration.HttpTimeout);

        AuthenticatedClient = HttpClientFactory.Create(
            Configuration.BaseUrl,
            Logger,
            Configuration.HttpTimeout,
            new AuthenticationHandler(AuthenticationContext));

        UsersClient = new UsersClient(Client);
        AuthenticationClient = new AuthenticationClient(Client);
        ProductsClient = new ProductsClient(Client);
        BasketClient = new BasketClient(AuthenticatedClient);

        UserSteps = new UserSteps(UsersClient, AuthenticationClient, AuthenticationContext);
        ProductSteps = new ProductSteps(ProductsClient);
        BasketSteps = new BasketSteps(BasketClient, AuthenticationContext);
    }

    [TearDown]
    public void TearDownApiClients()
    {
        AuthenticatedClient.Dispose();
        Client.Dispose();
    }

    protected async Task<ApiResponse<RegisterUserResponse>> RegisterUserAsync(
        User user,
        CancellationToken cancellationToken = default)
    {
        var response = await UserSteps.RegisterUserAsync(user, cancellationToken);

        LogCreatedUser(response, user.Email);

        return response;
    }

    protected async Task<ApiResponse<RegisterUserResponse>> RegisterUserAsync(
        RegisterUserRequest request,
        CancellationToken cancellationToken = default)
    {
        var response = await UsersClient.RegisterAsync(request, cancellationToken);

        LogCreatedUser(response, request.Email);

        return response;
    }

    protected async Task<string> LoginUserAsync(
        User user,
        CancellationToken cancellationToken = default)
        => await UserSteps.LoginUserAsync(user, cancellationToken);
    private void LogCreatedUser(ApiResponse<RegisterUserResponse> response, string email)
    {
        if (response.Data?.Data is { } createdUser)
        {
            Logger.Information($"Created test user '{email}' (id {createdUser.Id}).");
        }
    }
}