using SeniorCQCAssignment.Framework.Constants;
using SeniorCQCAssignment.Framework.Logging;

namespace SeniorCQCAssignment.Framework.HTTP;

public static class HttpClientFactory
{
    public static HttpClient Create(string baseUrl, ILogger logger, TimeSpan? timeout = null, HttpMessageHandler? innerHandler = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(baseUrl);

        var transportHandler = new HttpClientHandler();

        if (innerHandler is DelegatingHandler delegatingHandler)
        {
            delegatingHandler.InnerHandler = transportHandler;
        }

        var loggingHandler = new LoggingHandler(logger)
        {
            InnerHandler = innerHandler ?? transportHandler
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