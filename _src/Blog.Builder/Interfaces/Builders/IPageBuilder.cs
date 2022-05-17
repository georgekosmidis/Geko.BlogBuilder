using Blog.Builder.Models.Builders;
using Blog.Builder.Models.Templates;

namespace Blog.Builder.Interfaces.Builders;

/// <summary>
/// The service that builds the pages.
/// </summary>
internal interface IPageBuilder : IDisposable
{

    /// <summary>
    /// Builds the HTML of an interal layout
    /// </summary>
    /// <typeparam name="T">A type that inherits from <see cref="LayoutModelBase"/>.</typeparam>
    /// <param name="pageData">The data of the page.</param>
    /// <param name="bodyCards">A collection of cards that will be used as body.</param>
    /// <param name="rightColumnCards">The HTML of the cards of right column of the page.</param>
    /// <returns>Returns a string with the HTML of an internal layout, the relative URL and the date it was modified.</returns>
    string BuildInternalLayoutForIndex<T>(T pageData, IEnumerable<string> bodyCards, IEnumerable<string> rightColumnCards) where T : LayoutModelBase;

    /// <summary>
    /// Builds the HTML of an interal layout
    /// </summary>
    /// <typeparam name="T">A type that inherits from <see cref="LayoutModelBase"/>.</typeparam>
    /// <param name="pageData">The data of the page.</param>
    /// <param name="bodyHtml">The HTML that will be used as body.</param>
    /// <param name="rightColumnCards">The HTML of the cards of right column of the page.</param>
    /// <returns>Returns a string with the HTML of an internal layout, the relative URL and the date it was modified.</returns>
    string BuildInternalLayoutForPages<T>(T pageData, string bodyHtml, IEnumerable<string> rightColumnCards) where T : LayoutModelBase;

    /// <summary>
    /// Buids the HTML of the main layout
    /// </summary>
    /// <typeparam name="T">A type that inherits from <see cref="LayoutModelBase"/>.</typeparam>
    /// <param name="pageData">The data of the page.</param>
    /// <param name="bodyHtml">The compiled HTML of an inner layout.</param>
    /// <returns>Returns a <see cref="PageBuilderResult"/> with the HTML of the main layout, the relative URL and the date it was modified.</returns>
    PageBuilderResult BuildMainLayout<T>(T pageData, string bodyHtml) where T : LayoutModelBase;
}