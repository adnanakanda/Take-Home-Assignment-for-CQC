using SeniorCQCAssignment.Framework.Configuration;
using SeniorCQCAssignment.Framework.HTTP;

namespace SeniorCQCAssignment.Tests.Fixtures;

public abstract class ApiTestBase
{
    protected HttpClient HttpClient { get; private set; } = null!;

    protected TestConfiguration Configuration { get; private set; } = null!;

    [SetUp]
    public void ApiBaseSetUp()
    {
        Configuration = ConfigurationProvider.Load();

        HttpClient = HttpClientFactory.Create(Configuration.BaseUrl, Configuration.HttpTimeout);
    }

    [TearDown]
    public void ApiBaseTearDown() => HttpClient.Dispose();
}