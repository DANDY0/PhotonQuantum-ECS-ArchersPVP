using UnityEngine;
using System.Diagnostics;

public static class Logger
{
    public static LogLevel LogLevel { get; set; } = LogLevel.Info;

    public static void Log(string message)
    {
        if (LogLevel < LogLevel.Info) return;
        UnityEngine.Debug.Log(Format(message));
    }

    public static void Warn(string message)
    {
        if (LogLevel < LogLevel.Warning) return;
        UnityEngine.Debug.LogWarning(Format(message));
    }

    public static void Error(string message)
    {
        if (LogLevel < LogLevel.Error) return;
        UnityEngine.Debug.LogError(Format(message));
    }

    private static string Format(string message)
    {
        var frame = new StackTrace().GetFrame(2);
        var type = frame?.GetMethod()?.DeclaringType;
        var caller = type != null ? type.Name : "Unknown";
        return $"[{caller}] {message}";
    }
}

public enum LogLevel
{
    None = 0,
    Error = 1,
    Warning = 2,
    Info = 3
}