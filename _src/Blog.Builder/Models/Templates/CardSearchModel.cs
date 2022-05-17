namespace Blog.Builder.Models.Templates;

/// <summary>
/// Used for the (only one exists) search card.
/// Inherits all members of <see cref="CardModelBase"/>.
/// </summary>
public record class CardSearchModel : CardModelBase
{
    /// <summary>
    /// Ctor.
    /// </summary>
    /// <param name="appSettings">The appsettings.json model</param>
    public CardSearchModel(AppSettings appSettings) : base(appSettings)
    {
        TemplateDataModel = nameof(CardModelBase);
    }

    /// <summary>
    /// Validates what this object knows and throws an exception if something is wrong.
    /// Check the <see cref="Validate"/> source code for the validations.
    /// </summary>
    public new void Validate()
    {
        base.Validate();

        if (TemplateDataModel != nameof(CardSearchModel))
        {
            throw new Exception($"{nameof(TemplateDataModel)} must be {nameof(CardSearchModel)} for the type {nameof(CardSearchModel)}.");
        }
    }
}
