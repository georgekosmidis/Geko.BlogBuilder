using Blog.Builder.Interfaces;

namespace Blog.Builder.Services;

/// <summary>
/// I just need to write to the output, nothing fancy here
/// </summary>
internal class Logger : ILogger
{
    /// <summary>
    /// Wrapper of <see cref="Console.WriteLine()"/>.
    /// </summary>
    /// <param name="msg">The message to be written to the default output.</param>
    public void Log(string msg)
    {
        Console.WriteLine($"{DateTime.Now:HH:mm:ss.fff} - {msg}");
    }
}
