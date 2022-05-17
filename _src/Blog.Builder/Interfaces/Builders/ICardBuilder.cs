using Blog.Builder.Models.Templates;

namespace Blog.Builder.Interfaces.Builders;

/// <summary>
/// The service that builds cards.
/// </summary>
internal interface ICardBuilder : IDisposable
{
    /// <summary>
    /// Adds an article card to the collection of cards
    /// </summary>
    /// <param name="cardData">The <see cref="CardArticleModel"/> that conains all information.</param>
    /// <param name="datePublished">The date this artice was published (for ordering)</param>
    void AddArticleCard(CardArticleModel cardData, DateTime datePublished);

    /// <summary>
    /// Adds a card to the collection of cards. 
    /// The cardData of type <typeparamref name="T"/> that inherit from <see cref="CardModelBase"/>, must be passed.
    /// </summary>
    /// <typeparam name="T">A card type that inherits from <see cref="CardModelBase"/>.</typeparam>
    /// <param name="cardData">The data of the card.</param>
    void AddCard<T>(T cardData) where T : CardModelBase;

    /// <summary>
    /// Builds the HTML of the requested index page.
    /// </summary>
    /// <param name="pageIndex">The index of the page (e.g. 2 for index-page-3.html)</param>
    /// <param name="cardsPerPage">The number of cards per page.</param>
    /// <returns>A list of the HTML of the cards in ascending order.</returns>
    IEnumerable<string> GetBodyCardsHtml(int pageIndex, int cardsPerPage);

    /// <summary>
    /// Gets the total number of cards in the collection.
    /// The number of cards is calculated using the number of articles, the number of non-sticky cards
    /// and the number of sticky cards multiplied by the number of pages, since the sticky ones will 
    /// exist in every page.
    /// </summary>
    /// <param name="cardsPerPage">The number of cards per page. It will be used for the sticky cards calculation.</param>
    /// <returns>The total number of cards registered.</returns>
    int GetCardsNumber(int cardsPerPage);

    /// <summary>
    /// Compiles the HTML for the right column.
    /// </summary>
    /// <returns>A list of the HTML of the cards in ascending order.</returns>
    IEnumerable<string> GetRightColumnCardsHtml();
}
