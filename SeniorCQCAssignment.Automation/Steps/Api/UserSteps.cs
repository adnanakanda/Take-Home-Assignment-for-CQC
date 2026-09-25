using SeniorCQCAssignment.Automation.ApiClients;
using SeniorCQCAssignment.Automation.Context;
using SeniorCQCAssignment.Automation.Exceptions;
using SeniorCQCAssignment.Automation.Models.Api.Requests;
using SeniorCQCAssignment.Automation.Models.Api.Responses;
using SeniorCQCAssignment.Automation.Models.Domain;
using SeniorCQCAssignment.Framework.HTTP;

namespace SeniorCQCAssignment.Automation.Steps.Api;

public sealed class UserSteps
{
    private readonly UsersClient _usersClient;
    private readonly AuthenticationClient _authenticationClient;
    private readonly AuthenticationContext _authenticationContext;

    public UserSteps(UsersClient usersClient, AuthenticationClient authenticationClient, AuthenticationContext authenticationContext)
    {
        _usersClient = usersClient;
        _authenticationClient = authenticationClient;
        _authenticationContext = authenticationContext;
    }

    public async Task<ApiResponse<RegisterUserResponse>> RegisterUserAsync(User user)
    {
        var request = new RegisterUserRequest
        {
            Email = user.Email,
            Password = user.Password,
            PasswordRepeat = user.Password,
            SecurityQuestion = new SecurityQuestionRequest
            {
                Id = user.SecurityQuestionId
            },
            SecurityAnswer = user.SecurityAnswer
        };

        var response = await _usersClient.RegisterAsync(request);

        if (!response.IsSuccessStatusCode)
        {
            throw new ApiException($"User registration failed: {response.ResponseBody}");
        }

        return response;
    }

    public async Task<string> LoginUserAsync(User user)
    {
        var request = new LoginRequest
        {
            Email = user.Email,
            Password = user.Password
        };

        var response = await _authenticationClient.LoginAsync(request);

        if (!response.IsSuccessStatusCode)
        {
            throw new ApiException($"Login failed: {response.ResponseBody}");
        }

        var authentication = response.Data?.Authentication ?? throw new InvalidOperationException("Login succeeded but authentication data was not returned.");

        _authenticationContext.Token = authentication.Token ?? throw new InvalidOperationException("Authentication token was not returned.");

        _authenticationContext.BasketId = authentication.BasketId;

        _authenticationContext.Email = authentication.Email ?? user.Email;

        return _authenticationContext.Token;
    }
}