using System.Collections.Concurrent;

namespace SeniorCQCAssignment.Tests.Fixtures;

internal static class HttpContextStore
{
    private static readonly ConcurrentDictionary<string, ApiTestContext> Contexts = new();

    public static string Register(ApiTestContext context)
    {
        var token = Guid.NewGuid().ToString("N");

        Contexts[token] = context;

        return token;
    }

    public static void Unregister(string token)
    {
        Contexts.TryRemove(token, out _);
    }
}
