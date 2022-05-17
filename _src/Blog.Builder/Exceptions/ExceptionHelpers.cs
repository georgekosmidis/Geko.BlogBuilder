using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Blog.Builder.Exceptions;

/// <summary>
/// Helper methods that throw exception based on a specific failed check.
/// </summary>
/// <remarks>
/// todo: Right now an exception of type <see cref="Exception"/> is thrown. A custom exception would be a nice idea.
/// </remarks>
internal static class ExceptionHelpers
{
    /// <summary>
    /// Throws an exception if <paramref name="argument"/> is null or contains white spaces only.
    /// </summary>
    /// <param name="argument">The reference type argument to validate as non-null or filled with white spaces only.</param>
    /// <param name="paramName">The name of the parameter with which argument corresponds.</param>
    public static void ThrowIfNullOrWhiteSpace([NotNull] string? argument, [CallerArgumentExpression("argument")] string? paramName = null)
    {
        if (string.IsNullOrWhiteSpace(argument))
        {
            Throw($"{paramName} is null or filled with whitespaces only.");
        }
    }

    /// <summary>
    /// Throws an exception if <paramref name="argument"/> is null or default.
    /// </summary>
    /// <param name="argument">The reference type argument to validate as non-null.</param>
    /// <param name="paramName">The name of the parameter with which argument corresponds.</param>
    public static void ThrowIfNull([NotNull] object? argument, [CallerArgumentExpression("argument")] string? paramName = null)
    {
        if (argument is null)
        {
            Throw($"{paramName} is null.");
        }
    }

    /// <summary>
    /// Throws an exception if the <paramref name="argument"/> is an empty list.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="argument">The reference type argument to validate as a non-empty list.</param>
    /// <param name="paramName">The name of the parameter with which argument corresponds.</param>
    public static void ThrowIfNullOrEmpty<T>([NotNull] IEnumerable<T>? argument, [CallerArgumentExpression("argument")] string? paramName = null)
    {
        ThrowIfNull(argument, paramName);

        if (!argument.Any())
        {
            Throw($"{paramName} don't has any items.");
        }
    }

    /// <summary>
    /// Throws an exception if the <paramref name="argument"/> does not exists as a path in this system.
    /// </summary>
    /// <param name="argument">The reference type argument to validate as a path.</param>
    /// <param name="paramName">The name of the parameter with which argument corresponds.</param>
    public static void ThrowIfPathNotExists([NotNull] string? argument, [CallerArgumentExpression("argument")] string? paramName = null)
    {
        ThrowIfNullOrWhiteSpace(argument, paramName);

        if (!(Directory.Exists(argument) || File.Exists(argument)))
        {
            Throw($"Directory of file '{argument}' defined in '{paramName}' does not exists.");
        }
    }

    /// <summary>
    /// Throws an <seealso cref="Exception"/> when called.
    /// </summary>
    /// <param name="message">The message for the exception.</param>
    /// <exception cref="Exception">A generic exception for every <see cref="ExceptionHelpers"/> method.</exception>
    [DoesNotReturn]
    private static void Throw(string? message)
    {
        throw new Exception(message);
    }
}
