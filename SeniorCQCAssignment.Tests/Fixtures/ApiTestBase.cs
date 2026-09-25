using SeniorCQCAssignment.Automation.ApiClients;
using SeniorCQCAssignment.Automation.Context;
using SeniorCQCAssignment.Framework.Configuration;
using SeniorCQCAssignment.Framework.HTTP;
using SeniorCQCAssignment.Framework.Logging;

namespace SeniorCQCAssignment.Tests.Fixtures;

public abstract class ApiTestBase
{
    protected HttpClient Client { get; private set; } = null!;
    protected HttpClient AuthenticatedClient { get; private set; } = null!;
    protected UsersClient AuthenticatedUsersClient { get; private set; } = null!;
    protected TestConfiguration Configuration { get; private set; } = null!;
    protected UsersClient UsersClient { get; private set; } = null!;
    protected AuthenticationClient AuthenticationClient { get; private set; } = null!;
    protected ProductsClient ProductsClient { get; private set; } = null!;
    protected BasketClient BasketClient { get; private set; } = null!;
    protected AuthenticationContext AuthenticationContext { get; private set; } = null!;
    protected TestDataContext TestDataContext { get; private set; } = null!;
    protected ILogger Logger { get; private set; } = null!;

    [SetUp]
    public void Setup()
    {
        Logger = LoggerFactory.Create();

        Configuration = ConfigurationProvider.Load();

        AuthenticationContext = new AuthenticationContext();
        TestDataContext = new TestDataContext();

        Client = HttpClientFactory.Create(
            Configuration.BaseUrl,
            Logger,
            Configuration.HttpTimeout);

        UsersClient = new UsersClient(Client);
        AuthenticationClient = new AuthenticationClient(Client);
        ProductsClient = new ProductsClient(Client);

        AuthenticatedClient = HttpClientFactory.Create(
            Configuration.BaseUrl,
            Logger,
            Configuration.HttpTimeout,
            new AuthenticationHandler(AuthenticationContext));

        AuthenticatedUsersClient = new UsersClient(AuthenticatedClient);
        BasketClient = new BasketClient(AuthenticatedClient);
    }

    [TearDown]
    public async Task Cleanup()
    {
        foreach (var userId in TestDataContext.CreatedUserIds)
        {
            try
            {
                await AuthenticatedUsersClient.DeleteAsync(userId);
            }
            catch (Exception exception)
            {
                Logger.Error($"Failed to clean up user '{userId}': {exception.Message}");
            }
        }

        AuthenticatedClient?.Dispose();
        Client?.Dispose();
    }

    protected void TrackCreatedUser(int userId)
    {
        TestDataContext.CreatedUserIds.Add(userId);
    }
}