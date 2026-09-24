using SeniorCQCAssignment.Framework.Constants;
using SeniorCQCAssignment.Framework.Logging;

namespace SeniorCQCAssignment.Framework.HTTP;

public static class HttpClientFactory
{
    public static HttpClient Create(string baseUrl, ILogger logger, TimeSpan? timeout = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(baseUrl);

        var httpHandler = new HttpClientHandler();

        var loggingHandler = new LoggingHandler(logger)
        {
            InnerHandler = httpHandler
        };

        var client = new HttpClient(loggingHandler)
        {
            BaseAddress = new Uri(baseUrl),
            Timeout = timeout ?? Timeouts.Api
        };

        client.DefaultRequestHeaders.Accept.ParseAdd("application/json");

        return client;
    }
}