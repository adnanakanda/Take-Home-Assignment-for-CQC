using SeniorCQCAssignment.Framework.Constants;

namespace SeniorCQCAssignment.Framework.HTTP;

public static class HttpClientFactory
{
    public static HttpClient Create(string baseUrl, TimeSpan? timeout = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(baseUrl);

        var client = new HttpClient
        {
            BaseAddress = new Uri(baseUrl),
            Timeout = timeout ?? Timeouts.Api
        };

        client.DefaultRequestHeaders.Accept.ParseAdd("application/json");

        return client;
    }
}