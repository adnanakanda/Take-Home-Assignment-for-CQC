using SeniorCQCAssignment.Framework.Configuration;
using SeniorCQCAssignment.Framework.Logging;

namespace SeniorCQCAssignment.Tests.Fixtures;

public abstract class TestFixtureBase
{
    protected TestConfiguration Configuration { get; private set; } = null!;

    protected ILogger Logger { get; private set; } = null!;

    [SetUp]
    public void SetUpTestContext()
    {
        Logger = LoggerFactory.Create();

        Configuration = ConfigurationProvider.Load();
    }
}