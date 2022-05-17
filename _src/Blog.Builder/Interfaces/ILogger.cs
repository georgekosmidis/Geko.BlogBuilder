namespace Blog.Builder.Interfaces;

/// <summary>
/// For now it just writes to console
/// </summary>
internal interface ILogger
{
    /// <summary>
    /// Log a message.
    /// </summary>
    /// <param name="msg">The message to log</param>
    void Log(string msg);
}