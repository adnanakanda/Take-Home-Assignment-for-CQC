using SeniorCQCAssignment.Framework.HTTP;
using System.Net.Http.Json;
using System.Text.Json;

namespace SeniorCQCAssignment.Automation.ApiClients;

public abstract class ApiClientBase
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    protected ApiClientBase(HttpClient httpClient)
    {
        HttpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }

    protected HttpClient HttpClient { get; }

    protected async Task<ApiResponse<TResponse>> PostAsync<TRequest, TResponse>(
        string endpoint,
        TRequest request,
        CancellationToken cancellationToken = default)
    {
        var requestBody = JsonSerializer.Serialize(request, JsonOptions);

        using var response = await HttpClient.PostAsJsonAsync(
            endpoint,
            request,
            JsonOptions,
            cancellationToken);

        var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);

        TResponse? data = default;
        if (!string.IsNullOrWhiteSpace(responseBody))
        {
            try
            {
                data = JsonSerializer.Deserialize<TResponse>(responseBody, JsonOptions);
            }
            catch (JsonException)
            {
                data = default;
            }
        }

        return new ApiResponse<TResponse>
        {
            StatusCode = response.StatusCode,
            RequestBody = requestBody,
            ResponseBody = responseBody,
            Data = data
        };
    }
}