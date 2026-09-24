using SeniorCQCAssignment.Framework.Logging;

namespace SeniorCQCAssignment.Framework.HTTP;

public sealed class LoggingHandler : DelegatingHandler
{
    private readonly ILogger _logger;

    public LoggingHandler(ILogger logger) : base(new HttpClientHandler())
    {
        _logger = logger;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        _logger.Information($"{request.Method} {request.RequestUri}");

        if (request.Content is not null)
        {
            var requestBody = await request.Content.ReadAsStringAsync(cancellationToken);

            _logger.Debug($"Request Body: {MaskSensitiveData(requestBody)}");
        }

        var response = await base.SendAsync(request, cancellationToken);

        var responseBody = response.Content is null ? string.Empty : await response.Content.ReadAsStringAsync(cancellationToken);

        _logger.Debug($"Response {(int)response.StatusCode}: {MaskSensitiveData(responseBody)}");

        response.Content = new StringContent(responseBody, System.Text.Encoding.UTF8, "application/json");
        return response;
    }

    private static string MaskSensitiveData(string body)
    {
        if (string.IsNullOrWhiteSpace(body))
        {
            return body;
        }
        return body
        .Replace("\"token\":\"", "\"token\":\"***MASKED***")
        .Replace("\"password\":\"", "\"password\":\"***MASKED***");
    }
}