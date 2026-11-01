using SeniorCQCAssignment.Framework.Configuration;
using SeniorCQCAssignment.Tests.Constants;

namespace SeniorCQCAssignment.Tests.Unit;

public class ConfigurationProviderTests
{
    private const string EnvironmentVariable = "TestSettings__Environment";
    private const string BaseUrlVariable = "TestSettings__BaseUrl";
    private const string HttpTimeoutVariable = "TestSettings__HttpTimeoutSeconds";

    [Test]
    [Category(Categories.Unit)]
    public void Load_ReturnsValidConfiguration()
    {
        var configuration = ConfigurationProvider.Load();

        Assert.Multiple(() =>
        {
            Assert.That(configuration.BaseUrl, Is.Not.Empty, "No address to run against was configured.");
            Assert.That(Uri.TryCreate(configuration.BaseUrl, UriKind.Absolute, out var address), Is.True, $"The configured address '{configuration.BaseUrl}' is not an absolute address.");
            Assert.That(address!.Scheme, Is.EqualTo(Uri.UriSchemeHttp).Or.EqualTo(Uri.UriSchemeHttps), $"The configured address '{configuration.BaseUrl}' is not an HTTP address.");
            Assert.That(configuration.ExplicitWait, Is.GreaterThan(TimeSpan.Zero), "A run without an explicit wait would not wait for anything.");
            Assert.That(configuration.HttpTimeout, Is.GreaterThan(TimeSpan.Zero), "A run without an HTTP timeout would wait forever for an answer.");
        });
    }

    [Test]
    [Category(Categories.Unit)]
    public void Load_EnvironmentVariableOverridesConfiguration()
    {
        const string FromTheEnvironment = "http://host-from-the-environment:3001";

        var configuration = WithEnvironmentVariable(BaseUrlVariable, FromTheEnvironment, ConfigurationProvider.Load);

        Assert.That(configuration.BaseUrl, Is.EqualTo(FromTheEnvironment), "The environment variable did not win over the configuration files.");
    }

    [Test]
    [Category(Categories.Unit)]
    public void Load_UsesEnvironmentSpecificConfiguration()
    {
        var configuration = WithEnvironmentVariable(EnvironmentVariable, "ci", ConfigurationProvider.Load);

        Assert.Multiple(() =>
        {
            Assert.That(configuration.Environment, Is.EqualTo("ci"), "The run did not take the environment it was told to run in.");
            Assert.That(configuration.Headless, Is.True, "The settings of the ci environment were not read.");
        });
    }

    [Test]
    [Category(Categories.Unit)]
    public void Load_ThrowsExceptionForInvalidConfigurationValue()
    {
        var failure = Assert.Throws<InvalidOperationException>(
            () => WithEnvironmentVariable(HttpTimeoutVariable, "half a minute", ConfigurationProvider.Load));

        Assert.That(
            failure!.Message,
            Does.Contain("TestSettings:HttpTimeoutSeconds"),
            "A setting that cannot be read should say which setting it is, not quietly become the built-in default.");
    }

    private static void WithEnvironmentVariable(string name, string value, Action whileItIsSet)
    {
        var previous = Environment.GetEnvironmentVariable(name);

        Environment.SetEnvironmentVariable(name, value);

        try
        {
            whileItIsSet();
        }
        finally
        {
            Environment.SetEnvironmentVariable(name, previous);
        }
    }

    private static T WithEnvironmentVariable<T>(string name, string value, Func<T> whileItIsSet)
    {
        T result = default!;

        WithEnvironmentVariable(name, value, () => { result = whileItIsSet(); });

        return result;
    }
}