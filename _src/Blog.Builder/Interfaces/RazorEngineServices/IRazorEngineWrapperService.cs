using Blog.Builder.Models.Templates;
using RazorEngine.Templating;

namespace Blog.Builder.Interfaces.RazorEngineServices;

/// <summary>
/// A wrapper for the <see cref="IRazorEngineService"/>.
/// </summary>
internal interface IRazorEngineWrapperService : IDisposable
{
    /// <summary>
    /// Compiles and returns the html for the requested template
    /// </summary>
    /// <typeparam name="T">Any template model that inherits from a <see cref="ModelBase"/>.</typeparam>
    /// <param name="data">The data for the template</param>
    /// <returns>Returns the compiled HTML of the template assigned to <typeparamref name="T"/>
    /// and the data of the <paramref name="data"/> argument.</returns>
    string RunCompile<T>(T data) where T : ModelBase;
}