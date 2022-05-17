using Blog.Builder.Exceptions;
using Blog.Builder.Interfaces;
using Blog.Builder.Models.Templates;
using RazorEngine.Templating;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;

namespace Blog.Builder.Services.RazorEngineServices;

/// <inheritdoc/>
internal class TemplateManager : ITemplateManager
{
    private readonly ITemplateProvider _templateProvider;
    private readonly ConcurrentDictionary<ITemplateKey, ITemplateSource> _dynamicTemplates = new();
    /// <summary>
    /// Custom Template Manager for the <see cref="RazorEngine"/>.
    /// </summary>
    /// <param name="templateProvider">The template provider that hosts paths and html of all templates. See <see cref="TemplateProvider"/>.</param>
    public TemplateManager(ITemplateProvider templateProvider)
    {
        _templateProvider = templateProvider;
    }

    /// <inheritdoc/>
    public ITemplateSource Resolve(ITemplateKey key)
    {
        string template = _templateProvider.GetHtml(key.Name);

        return new LoadedTemplateSource(template);
    }

    /// <inheritdoc/>
    public ITemplateKey GetKey(string name, ResolveType resolveType, ITemplateKey context)
    {
        ExceptionHelpers.ThrowIfNullOrWhiteSpace(name);

        string? path = _templateProvider.GetPath(name);
        ExceptionHelpers.ThrowIfPathNotExists(path);

        // template is specified by full path
        return new FullPathTemplateKey(name, path, resolveType, context);
    }

    /// <summary>
    /// Throws a <see cref="NotImplementedException"/>.
    /// This method cannot be used with a custom implatemantation of a <see cref="ITemplateManager"/>.
    /// </summary>
    /// <param name="key">Not to be used.</param>
    /// <param name="source">Not to be used.</param>
    /// <exception cref="NotImplementedException">Throws this exception whenever is called.</exception>
    [DoesNotReturn]
    public void AddDynamic(ITemplateKey key, ITemplateSource source)
    {
        throw new NotImplementedException($"{nameof(AddDynamic)} should never be called under this custom implementation." +
$"All the templates are solved based on their template model (e.g. {nameof(LayoutIndexModel)}");
    }
}
