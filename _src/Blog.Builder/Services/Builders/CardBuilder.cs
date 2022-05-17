using Blog.Builder.Exceptions;
using Blog.Builder.Interfaces.Builders;
using Blog.Builder.Interfaces.RazorEngineServices;
using Blog.Builder.Models;
using Blog.Builder.Models.Builders;
using Blog.Builder.Models.Templates;

namespace Blog.Builder.Services.Builders;

//todo: honour single responsibility
/// <inheritdoc/>
internal class CardBuilder : ICardBuilder
{
    private readonly IRazorEngineWrapperService _templateEngine;
    private static readonly List<ArticleCardBuilderResult> ArticleCards = new();
    private static readonly List<OtherCardBuilderResult> OtherCards = new();

    public CardBuilder(IRazorEngineWrapperService templateEngine)
    {
        ArgumentNullException.ThrowIfNull(templateEngine);

        _templateEngine = templateEngine;
    }

    /// <summary>
    /// Creates the HTML of card based on a type that inherits from <see cref="CardModelBase"/>.
    /// </summary>
    /// <typeparam name="T">Any type that inherits from <see cref="CardModelBase"/></typeparam>
    /// <param name="cardData">The data of the card, necessary for building the HTML of a card.</param>
    /// <returns>The HTML of the card.</returns>
    private string CreateCardHtml<T>(T cardData) where T : CardModelBase
    {
        ArgumentNullException.ThrowIfNull(cardData);
        cardData.Validate();

        return _templateEngine.RunCompile(cardData);
    }

    /// <inheritdoc/>
    public void AddCard<T>(T cardDataBase) where T : CardModelBase
    {
        ExceptionHelpers.ThrowIfNull(cardDataBase);

        var cardHtml = CreateCardHtml(cardDataBase);
        ExceptionHelpers.ThrowIfNullOrWhiteSpace(cardHtml);

        //Add the card to the collection of cards
        OtherCards.Add(new OtherCardBuilderResult
        {
            CardHtml = cardHtml,
            Position = cardDataBase.Position,
            IsSticky = cardDataBase.IsSticky,
            RightColumnPosition = cardDataBase.RightColumnPosition
        });
    }

    /// <inheritdoc/>
    public void AddArticleCard(CardArticleModel cardData, DateTime datePublished)
    {
        var cardHtml = CreateCardHtml(cardData);
        ExceptionHelpers.ThrowIfNullOrWhiteSpace(cardHtml);

        ArticleCards.Add(new ArticleCardBuilderResult
        {
            CardHtml = cardHtml,
            DateCreated = datePublished
        });
    }

    /// <inheritdoc/>
    public IEnumerable<string> GetRightColumnCardsHtml()
    {
        var cards = OtherCards
                             .Where(x => x.RightColumnPosition > -1)
                             .OrderBy(x => x.RightColumnPosition)
                             .Select(x => x.CardHtml);

        return cards;
    }

    /// <inheritdoc/>
    public IEnumerable<string> GetBodyCardsHtml(int pageIndex, int cardsPerPage)
    {
        var cards = ArticleCards.OrderByDescending(x => x.DateCreated)
                                .Select(x => x.CardHtml)
                                .ToList();
        var stickyCardsNum = OtherCards.Count(x => x.IsSticky);

        //add the none-sticky cards to their correct position
        foreach (var card in OtherCards.Where(x => !x.IsSticky).OrderBy(x => x.Position))
        {
            if (card.Position > cards.Count)
            {
                cards.Add(card.CardHtml);
            }
            else
            {
                cards.Insert(card.Position, card.CardHtml);
            }
        }

        //Select the cards that will play a role in the paging,
        // sticky cards will appear in every page anyway so they can be excluded
        if (stickyCardsNum >= cardsPerPage)
        {
            throw new Exception($"Number of cards per page ({nameof(AppSettings)}.{nameof(AppSettings.CardsPerPage)}) must be bigger than the number of sticky cards (" +
                $"check property {nameof(CardModelBase.IsSticky)} in all additional cards).");
        }
        cards = cards.Skip(pageIndex * (cardsPerPage - stickyCardsNum)).Take(cardsPerPage - stickyCardsNum).ToList();

        //Don't create pages with just sticky cards, it makes no sense
        // this action should have been avoided from GetCardsNumber method
        if (cards.Count == 0)
        {
            throw new Exception($"A request to build a page with just sticky cards is not valid. This action should have been avoided by the {nameof(this.GetCardsNumber)} method.");
        }

        //Add the sticky card to their correct position
        foreach (var card in OtherCards.Where(x => x.IsSticky).OrderBy(x => x.Position))
        {
            if (card.Position > cards.Count)
            {
                cards.Add(card.CardHtml);
            }
            else
            {
                cards.Insert(card.Position, card.CardHtml);
            }
        }

        //Return the html
        return cards;
    }

    /// <inheritdoc/>
    public int GetCardsNumber(int cardsPerPage)
    {
        var stickyCardsNum = OtherCards.Count(x => x.IsSticky);
        if (stickyCardsNum >= cardsPerPage)
        {
            throw new Exception($"Number of cards per page ({nameof(AppSettings)}.{nameof(AppSettings.CardsPerPage)}) must be bigger than the number of sticky cards (" +
                $"check property {nameof(CardModelBase.IsSticky)} in all additional cards).");
        }

        //Calculate the number of pages withouth the sticky cards,
        // they will present in each page anyway
        var totalPages = Math.Ceiling(ArticleCards.Count / (decimal)(cardsPerPage - stickyCardsNum));

        //Return the number of article cards
        // plus the number of additional cards that are not sticky
        // plus the sticky cards that will exist in every page
        var totalCount = ArticleCards.Count
                            + OtherCards.Count(x => !x.IsSticky)
                            + (OtherCards.Count(x => x.IsSticky) * (int)totalPages);

        //If last page contains just the sticky cards, then do not create a page just for that
        if (totalCount % cardsPerPage <= stickyCardsNum)
        {
            totalCount -= totalCount % cardsPerPage;
        }

        return totalCount;
    }

    public void Dispose()
    {
        _templateEngine.Dispose();
    }
}
