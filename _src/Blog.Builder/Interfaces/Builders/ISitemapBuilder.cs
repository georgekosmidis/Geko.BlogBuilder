namespace Blog.Builder.Interfaces.Builders;

/// <summary>
/// The service that builds the sitmap.xml
/// </summary>
internal interface ISitemapBuilder : IDisposable
{
    /// <summary>
    /// Adds a URL to the sitemap collection.
    /// </summary>
    /// <param name="relativeUrl">The relative URL of the page.</param>
    /// <param name="dateModified">The last modification date.</param>
    void Add(string relativeUrl, DateTime dateModified);

    /// <summary>
    /// Builds the sitemap based an all registered urls in a private static collection.
    /// See also <see cref="Models.Templates.LayoutSitemapModel"/>.
    /// </summary>
    void Build();
}