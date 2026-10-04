using SeniorCQCAssignment.Automation.Models.Api.Requests;
using SeniorCQCAssignment.Automation.Models.Api.Responses;
using SeniorCQCAssignment.Automation.Models.Domain;
using SeniorCQCAssignment.Framework.Configuration;
using SeniorCQCAssignment.Framework.HTTP;
using SeniorCQCAssignment.Framework.Logging;

namespace SeniorCQCAssignment.Tests.Fixtures;

public abstract class TestFixtureBase
{
    protected TestConfiguration Configuration { get; private set; } = null!;

    protected ILogger Logger { get; private set; } = null!;

    protected ApiTestContext Api { get; private set; } = null!;

    [SetUp]
    public void SetUpTestContext()
    {
        Logger = LoggerFactory.Create();

        Configuration = ConfigurationProvider.Load();

        Api = new ApiTestContext(Configuration, Logger);
    }

    [TearDown]
    public void TearDownTestContext() => Api.Dispose();

    protected async Task<ApiResponse<RegisterUserResponse>> RegisterUserAsync(
        User user,
        CancellationToken cancellationToken = default)
    {
        var response = await Api.UserSteps.RegisterUserAsync(user, cancellationToken);

        LogCreatedUser(response, user.Email);

        return response;
    }

    protected async Task<ApiResponse<RegisterUserResponse>> RegisterUserAsync(
        RegisterUserRequest request,
        CancellationToken cancellationToken = default)
    {
        Logger.Information($"Registering a hand built request for '{request.Email}' over the API.");

        var response = await Api.UsersClient.RegisterAsync(request, cancellationToken);

        Logger.Information($"The API answered registration for '{request.Email}' with HTTP {(int)response.StatusCode}.");

        LogCreatedUser(response, request.Email);

        return response;
    }

    protected async Task<string> LoginUserAsync(
        User user,
        CancellationToken cancellationToken = default)
        => await Api.UserSteps.LoginUserAsync(user, cancellationToken);

    private void LogCreatedUser(ApiResponse<RegisterUserResponse> response, string email)
    {
        if (response.Data?.Data is { } createdUser)
        {
            Logger.Information($"Created test user '{email}' (id {createdUser.Id}).");
        }
    }
}