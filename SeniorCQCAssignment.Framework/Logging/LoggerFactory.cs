namespace SeniorCQCAssignment.Framework.Logging;

public static class LoggerFactory
{
    public static ILogger Create() => new TestLogger();
}