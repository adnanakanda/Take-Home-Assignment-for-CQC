namespace SeniorCQCAssignment.Framework.Constants;

public static class Timeouts
{
    public static readonly TimeSpan Short = TimeSpan.FromSeconds(5);

    public static readonly TimeSpan Default = TimeSpan.FromSeconds(10);

    public static readonly TimeSpan Long = TimeSpan.FromSeconds(30);

    public static readonly TimeSpan Api = TimeSpan.FromSeconds(60);
}