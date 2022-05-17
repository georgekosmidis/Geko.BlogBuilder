using Blog.Builder.Models.Templates;

namespace Blog.Builder.Interfaces;

/// <summary>
/// The service that does the card processing
/// </summary>
internal interface ICardProcessor : IDisposable
{
    /// <summary>
    /// Processes the data of a card (not article card).
    /// </summary>
    /// <param name="directory">The directory in which all necessary files exist.</param>
    Task ProcessCardAsync(string directory);

    /// <summary>
    /// Processes article cards.
    /// </summary>
    /// <param name="cardData">A <see cref="CardArticleModel"/> that contains all necessary information for an article.</param>
    /// <param name="datePublished">The date this article has been published</param>
    void ProcessArticleCard(CardArticleModel cardData, DateTime datePublished);

    /// <summary>
    /// Calls the card builder to calculate the number of cards available.
    /// </summary>
    /// <param name="cardsPerPage">The cards per page.</param>
    /// <returns>Returns the total number of cards.</returns>
    int GetCardsNumber(int cardsPerPage);

    /// <summary>
    /// Calls the card builder to prepare the HTML for the body of the index pages.
    /// </summary>
    /// <param name="pageIndex">The index of the page (e.g. 2 for index-page-3.html)</param>
    /// <param name="cardsPerPage">THe number of cards per page.</param>
    /// <returns>A list of the HTML of the cards in ascending order.</returns>
    IEnumerable<string> GetBodyCardsHtml(int pageIndex, int cardsPerPage);

    /// <summary>
    /// Compiles the HTML for the right column.
    /// </summary>
    /// <returns>A list of the HTML of the cards in ascending order.</returns>
    IEnumerable<string> GetRightColumnCardsHtml();

}