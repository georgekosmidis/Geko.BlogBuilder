namespace Blog.Builder.Models.Builders;

/// <summary>
/// The result of the <see cref="Services.Builders.CardBuilder.AddCard{T}(T)"/> method.
/// </summary>
internal record class OtherCardBuilderResult
{
    /// <summary>
    /// The HTML of the card.
    /// </summary>
    public string CardHtml { get; init; } = default!;

    //The position of the card in the grid.
    public int Position { get; init; }

    /// <summary>
    /// The stickiness of the card (exists in the same <see cref="Position"/> in every page).
    /// </summary>
    public bool IsSticky { get; init; }

    /// <summary>
    /// The position of this card in the right column for the layouts that support it.
    /// Default is -1 which means "do not add to the right column".
    /// </summary>
    public int RightColumnPosition { get; init; } = -1;
}