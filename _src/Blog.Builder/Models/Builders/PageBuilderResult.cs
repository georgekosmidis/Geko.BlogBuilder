namespace Blog.Builder.Models.Builders;

/// <summary>
/// The result of a <see cref="Services.Builders.PageBuilder"/> method.
/// </summary>
internal record class PageBuilderResult
{
    /// <summary>
    /// The final HTML that was build for this page.
    /// </summary>
    public string FinalHtml { get; init; } = default!;
    /// <summary>
    /// The relative URL of this page.
    /// </summary>
    public string RelativeUrl { get; init; } = default!;
    /// <summary>
    /// The date this page was last modified.
    /// </summary>
    public DateTime DateModified { get; init; } = DateTime.MaxValue;

}
