using Blog.Builder.Exceptions;
using Blog.Builder.Interfaces.Builders;
using Blog.Builder.Interfaces.RazorEngineServices;
using Blog.Builder.Models;
using Blog.Builder.Models.Templates;
using Microsoft.Extensions.Options;

namespace Blog.Builder.Services.Builders;

/// <inheritdoc/>
internal class SitemapBuilder : ISitemapBuilder
{
    private readonly IRazorEngineWrapperService _templateEngine;
    private readonly LayoutSitemapModel sitemapModel;

    public SitemapBuilder(IRazorEngineWrapperService templateService, IOptions<AppSettings> options)
    {
        ArgumentNullException.ThrowIfNull(templateService);
        ArgumentNullException.ThrowIfNull(options);

        sitemapModel = new(options.Value);

        _templateEngine = templateService;
    }

    /// <inheritdoc/>
    public void Build()
    {
        ExceptionHelpers.ThrowIfNullOrEmpty(sitemapModel.Urls);

        var sitemapPageXml = _templateEngine.RunCompile(sitemapModel);
        ExceptionHelpers.ThrowIfNullOrWhiteSpace(sitemapPageXml);

        var sitemap = Path.Combine(Globals.OutputFolderPath, Globals.GoogleSitemap);
        File.WriteAllText(sitemap, sitemapPageXml);
    }

    /// <inheritdoc/>
    public void Add(string relativeUrl, DateTime dateModified)
    {
        ArgumentNullException.ThrowIfNull(relativeUrl);
        ArgumentNullException.ThrowIfNull(dateModified);

        sitemapModel.Add(relativeUrl, dateModified);
    }

    public void Dispose()
    {
        _templateEngine.Dispose();
    }
}
