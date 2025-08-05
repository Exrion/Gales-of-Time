using System.Runtime.CompilerServices;
using UnityEngine;

public interface ILogger
{
    void LogInfo(           string message,
        [CallerMemberName]  string member = "",
        [CallerFilePath]    string file = "",
        [CallerLineNumber]  int line = 0);
    void LogWarning(        string message,
        [CallerMemberName]  string member = "",
        [CallerFilePath]    string file = "",
        [CallerLineNumber]  int line = 0);
    void LogError(          string message,
        [CallerMemberName]  string member = "",
        [CallerFilePath]    string file = "",
        [CallerLineNumber]  int line = 0);
}

public class UnityLogger : ILogger
{
    public void LogInfo(    string message,
        [CallerMemberName]  string member = "",
        [CallerFilePath]    string file = "",
        [CallerLineNumber]  int line = 0) => Debug.Log($"[{System.IO.Path.GetFileName(file)}:{line} - {member}] INFO: {message}");
    public void LogWarning(    string message,
        [CallerMemberName]  string member = "",
        [CallerFilePath]    string file = "",
        [CallerLineNumber]  int line = 0) => Debug.Log($"[{System.IO.Path.GetFileName(file)}:{line} - {member}] WARN: {message}");
    public void LogError(    string message,
        [CallerMemberName]  string member = "",
        [CallerFilePath]    string file = "",
        [CallerLineNumber]  int line = 0) => Debug.Log($"[{System.IO.Path.GetFileName(file)}:{line} - {member}] ERROR: {message}");
}

public class FileLogger : ILogger
{
    public void LogError(string message, [CallerMemberName] string member = "", [CallerFilePath] string file = "", [CallerLineNumber] int line = 0)
    {
        throw new System.NotImplementedException();
    }

    public void LogInfo(string message, [CallerMemberName] string member = "", [CallerFilePath] string file = "", [CallerLineNumber] int line = 0)
    {
        throw new System.NotImplementedException();
    }

    public void LogWarning(string message, [CallerMemberName] string member = "", [CallerFilePath] string file = "", [CallerLineNumber] int line = 0)
    {
        throw new System.NotImplementedException();
    }
}