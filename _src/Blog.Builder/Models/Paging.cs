namespace Blog.Builder.Models;

/// <summary>
/// The paging used in index pages
/// </summary>
public record class Paging
{
    /// <summary>
    /// The number of cards per page. Default is 9. 
    /// </summary>
    public int CardsPerPage { get; set; } = 9;

    /// <summary>
    /// The total number of cards registered for this website.
    /// </summary>
    public int TotalCardsCount { get; set; }

    /// <summary>
    /// The current page index (e.g. 2 for index-page-3.html).
    /// </summary>
    public int CurrentPageIndex { get; set; }

    /// <summary>
    /// The calculated page count.
    /// </summary>
    public int PageCount => (int)Math.Ceiling(TotalCardsCount / (decimal)CardsPerPage);
}
