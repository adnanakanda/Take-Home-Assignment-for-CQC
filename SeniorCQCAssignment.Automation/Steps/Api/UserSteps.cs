using SeniorCQCAssignment.Automation.ApiClients;
using SeniorCQCAssignment.Automation.Models.Api.Requests;
using SeniorCQCAssignment.Automation.Models.Domain;

namespace SeniorCQCAssignment.Automation.Api.Steps;

public sealed class UserSteps
{
    private readonly UsersClient _usersClient;
    private readonly AuthenticationClient _authenticationClient;

    public UserSteps(UsersClient usersClient, AuthenticationClient authenticationClient)
    {
        _usersClient = usersClient;
        _authenticationClient = authenticationClient;
    }

    public async Task RegisterUserAsync(User user)
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
            throw new Exception($"User registration failed: {response.ResponseBody}");
        }
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
            throw new Exception($"Login failed: {response.ResponseBody}");
        }

        return response.Data?
            .Authentication?
            .Token
            ?? throw new InvalidOperationException("Login succeeded but token was not returned.");
    }
}