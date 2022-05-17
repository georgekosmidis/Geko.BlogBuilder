namespace Blog.Builder.Models.Templates;

/// <summary>
/// The base template model for cards. Cards are used only in the index pages for now.
/// </summary>
public record class CardModelBase : ModelBase
{
    /// <summary>
    /// Ctor.
    /// </summary>
    /// <param name="appSettings">The appsettings.json model</param>
    public CardModelBase(AppSettings appSettings) : base(appSettings)
    {
        TemplateDataModel = nameof(CardModelBase);
        LinkTarget = "_blank";
        RightColumnPosition = -1;
    }

    /// <summary>
    /// The title of the card. Can be empty if the card is a <see cref="CardImageModel"/> or a <see cref="CardSearchModel"/>.
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// The description of the card. Can be empty if the card is a <see cref="CardImageModel"/> or a <see cref="CardSearchModel"/>.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// The footer of the card. Can be empty if the card is a <see cref="CardImageModel"/> or a <see cref="CardSearchModel"/>.
    /// </summary>
    public string? Footer { get; set; }

    /// <summary>
    /// The link of the card. Can be empty if the card is a <see cref="CardSearchModel"/>.
    /// </summary>
    public string? Link { get; set; }

    /// <summary>
    /// The target for the link. Defaults to "_blank";
    /// </summary>
    public string LinkTarget { get; set; }

    /// <summary>
    /// The url for the image of the card. Can be empty if the card is a <see cref="CardSearchModel"/>.
    /// </summary>
    public string? ImageUrl { get; set; }

    /// <summary>
    /// The position of the card in the grid.
    /// </summary>
    public int Position { get; set; }

    /// <summary>
    /// A boolean signaling if the card should appear in the <see cref="Position"/> in every page.
    /// </summary>
    public bool IsSticky { get; set; }

    /// <summary>
    /// The position of the card in the pages that have a right column.
    /// -1 means do not add.
    /// </summary>
    public int RightColumnPosition { get; set; }

    /// <summary>
    /// Validates what this object knows and throws an exception if something is wrong.
    /// Check the <see cref="Validate"/> source code for the validations.
    /// </summary>
    public new void Validate()
    {
        base.Validate();

        if (TemplateDataModel == nameof(CardModelBase))
        {
            throw new Exception($"{nameof(TemplateDataModel)} cannot be of base type {nameof(CardModelBase)}.");
        }
    }
}
