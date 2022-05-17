using Blog.Builder.Models.Templates;

namespace Blog.Builder.Interfaces;

/// <summary>
/// The service that does page processing
/// </summary>
internal interface IPageProcessor : IDisposable
{
    /// <summary>
    /// It processes data for a page that exists in a directory, e.g. standalones or articles.
    /// </summary>
    /// <typeparam name="T">The type of the model for the template.</typeparam>
    /// <param name="directory">The directory where page data lies.</param>
    void ProcessPage<T>(string directory) where T : LayoutModelBase;

    /// <summary>
    /// Processes data for index pages only (index.html, index-page-2.html etc)
    /// </summary>
    /// <param name="pageData">A <see cref="LayoutIndexModel"/> that contains all necessary information for the index page.</param>
    /// <param name="cardsPerPage">The number of cards per page.</param>
    void ProcessIndex(LayoutIndexModel pageData, int cardsPerPage);
}