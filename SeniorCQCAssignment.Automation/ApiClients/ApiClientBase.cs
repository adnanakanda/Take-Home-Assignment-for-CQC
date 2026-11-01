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

        var apiResponse = new ApiResponse<TResponse>
        {
            StatusCode = response.StatusCode,
            RequestBody = requestBody,
            ResponseBody = responseBody,
            Data = data
        };

        Record(apiResponse);

        return apiResponse;
    }

    protected async Task<ApiResponse<TResponse>> GetAsync<TResponse>(string endpoint, CancellationToken cancellationToken = default)
    {
        using var response = await HttpClient.GetAsync(endpoint, cancellationToken);
        var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);
        TResponse? data = default;

        if (!string.IsNullOrWhiteSpace(responseBody))
        {
            try
            {
                data =
                    JsonSerializer.Deserialize<TResponse>(
                        responseBody,
                        JsonOptions);
            }
            catch (JsonException)
            {
                data = default;
            }
        }

        var apiResponse = new ApiResponse<TResponse>
        {
            StatusCode = response.StatusCode,
            RequestBody = string.Empty,
            ResponseBody = responseBody,
            Data = data
        };

        Record(apiResponse);

        return apiResponse;
    }

    protected async Task<ApiResponse<TResponse>> DeleteAsync<TResponse>(string endpoint, CancellationToken cancellationToken = default)
    {
        using var response = await HttpClient.DeleteAsync(endpoint, cancellationToken);
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

        var apiResponse = new ApiResponse<TResponse>
        {
            StatusCode = response.StatusCode,
            RequestBody = string.Empty,
            ResponseBody = responseBody,
            Data = data
        };

        Record(apiResponse);

        return apiResponse;
    }

    private void Record<TResponse>(ApiResponse<TResponse> response)
    {
        if (HttpClient.DefaultRequestHeaders.TryGetValues("X-Test-Context", out var values))
        {
            if (values.FirstOrDefault() is { } token)
            {
                var storeType = Type.GetType("SeniorCQCAssignment.Tests.Fixtures.HttpContextStore, SeniorCQCAssignment.Tests");

                if (storeType is null)
                {
                    return;
                }

                var tryGet = storeType.GetMethod("TryGet", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);

                if (tryGet?.Invoke(null, [token]) is not { } context)
                {
                    return;
                }

                var record = context.GetType().GetMethods(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public)
                    .FirstOrDefault(m => m.Name == "RecordResponse" && m.GetParameters().Length == 1 && !m.GetParameters()[0].ParameterType.IsGenericTypeDefinition);

                if (record is null)
                {
                    return;
                }

                var generic = record.GetParameters()[0].ParameterType.GetGenericTypeDefinition() == typeof(ApiResponse<>)
                    ? record
                    : record.MakeGenericMethod(typeof(object));

                try
                {
                    generic.Invoke(context, [response]);
                }
                catch
                {
                }
            }
        }
    }
}