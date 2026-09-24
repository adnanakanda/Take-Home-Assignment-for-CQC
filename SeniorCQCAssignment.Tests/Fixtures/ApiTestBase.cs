using SeniorCQCAssignment.Automation.ApiClients;
using SeniorCQCAssignment.Framework.Configuration;
using SeniorCQCAssignment.Framework.HTTP;
using SeniorCQCAssignment.Framework.Logging;

namespace SeniorCQCAssignment.Tests.Fixtures;

public abstract class ApiTestBase
{
    protected HttpClient Client { get; private set; } = null!;
    protected TestConfiguration Configuration { get; private set; } = null!;
    protected UsersClient UsersClient { get; private set; } = null!;
    protected AuthenticationClient AuthenticationClient { get; private set; } = null!;
    protected ILogger Logger { get; private set; } = null!;


    [SetUp]
    public void Setup()
    {
        Logger = LoggerFactory.Create();

        Configuration = ConfigurationProvider.Load();

        Client = HttpClientFactory.Create(Configuration.BaseUrl, Logger, Configuration.HttpTimeout);

        UsersClient = new UsersClient(Client);

        AuthenticationClient = new AuthenticationClient(Client);
    }


    [TearDown]
    public void Cleanup()
    {
        Client.Dispose();
    }
}