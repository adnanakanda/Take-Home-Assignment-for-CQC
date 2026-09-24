namespace SeniorCQCAssignment.Framework.Logging;

public interface ILogger
{
    void Log(LogLevel level, string message);

    void Information(string message);

    void Warning(string message);

    void Error(string message, Exception? exception = null);

    void Debug(string message);
}