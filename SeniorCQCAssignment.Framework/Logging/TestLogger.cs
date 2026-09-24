namespace SeniorCQCAssignment.Framework.Logging;

public sealed class TestLogger : ILogger
{
    public void Log(LogLevel level, string message)
    {
        Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [{level}] {message}");
    }

    public void Information(string message) => Log(LogLevel.Information, message);


    public void Warning(string message) => Log(LogLevel.Warning, message);


    public void Debug(string message) => Log(LogLevel.Debug, message);


    public void Error(string message, Exception? exception = null)
    {
        Log(LogLevel.Error, exception is null ? message : $"{message}{Environment.NewLine}{exception}");
    }
}