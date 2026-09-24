using SeniorCQCAssignment.Automation.Models.Api.Requests;
using SeniorCQCAssignment.Automation.Models.Api.Responses;
using SeniorCQCAssignment.Framework.HTTP;

namespace SeniorCQCAssignment.Automation.ApiClients;

public sealed class UsersClient : ApiClientBase
{
    private const string RegisterEndpoint = "/api/Users/";

    public UsersClient(HttpClient client)
        : base(client)
    {
    }

    public Task<ApiResponse<RegisterUserResponse>> RegisterAsync(RegisterUserRequest request, CancellationToken cancellationToken = default)
        => PostAsync<RegisterUserRequest, RegisterUserResponse>(RegisterEndpoint, request, cancellationToken);
}