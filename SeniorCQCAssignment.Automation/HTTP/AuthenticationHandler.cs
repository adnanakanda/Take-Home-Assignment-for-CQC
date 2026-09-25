using SeniorCQCAssignment.Automation.Context;
using System.Net.Http.Headers;

namespace SeniorCQCAssignment.Framework.HTTP;

public sealed class AuthenticationHandler : DelegatingHandler
{
    private readonly AuthenticationContext _authenticationContext;

    public AuthenticationHandler(AuthenticationContext authenticationContext)
    {
        _authenticationContext = authenticationContext;
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (!string.IsNullOrWhiteSpace(_authenticationContext.Token))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _authenticationContext.Token);
        }

        return base.SendAsync(request, cancellationToken);
    }
}