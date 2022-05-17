namespace Blog.Builder.Models.Builders;

/// <summary>
/// The result of the <see cref="Services.Builders.CardBuilder.AddArticleCard(Blog.Builder.Models.Templates.CardArticleModel, DateTime)"/> method.
/// </summary>
internal record class ArticleCardBuilderResult
{
    /// <summary>
    /// The card HTML.
    /// </summary>
    public string CardHtml { get; init; } = default!;

    /// <summary>
    /// The datetime this article was created (for ordering).
    /// </summary>
    public DateTime DateCreated { get; init; }
}