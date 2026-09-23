using SeniorCQCAssignment.Automation.Models.Api.Requests;
using SeniorCQCAssignment.Automation.Models.Api.Responses;
using SeniorCQCAssignment.Framework.HTTP;

namespace SeniorCQCAssignment.Automation.ApiClients;

public sealed class AuthenticationClient : ApiClientBase
{
    private const string LoginEndpoint = "/rest/user/login";

    public AuthenticationClient(HttpClient httpClient)
        : base(httpClient)
    {
    }

    public Task<ApiResponse<LoginResponse>> LoginAsync(
        LoginRequest request,
        CancellationToken cancellationToken = default) =>
        PostAsync<LoginRequest, LoginResponse>(
            LoginEndpoint,
            request,
            cancellationToken);
}