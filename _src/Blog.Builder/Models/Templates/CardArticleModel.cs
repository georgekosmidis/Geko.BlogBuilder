using Blog.Builder.Exceptions;

namespace Blog.Builder.Models.Templates;

/// <summary>
/// A card template model that displays an article.
/// </summary>
public record class CardArticleModel : CardModelBase
{
    /// <summary>
    /// Ctor.
    /// </summary>
    /// <param name="appSettings">The appsettings.json model.</param>
    public CardArticleModel(AppSettings appSettings) : base(appSettings)
    {
        TemplateDataModel = nameof(CardArticleModel);
    }

    /// <summary>
    /// Validates what this object knows and throws an exception if something is wrong.
    /// Check the <see cref="Validate"/> source code for the validations.
    /// </summary>
    public new void Validate()
    {
        base.Validate();

        ExceptionHelpers.ThrowIfNullOrWhiteSpace(Title);
        ExceptionHelpers.ThrowIfNullOrWhiteSpace(Description);
        ExceptionHelpers.ThrowIfNullOrWhiteSpace(Footer);
        ExceptionHelpers.ThrowIfNullOrWhiteSpace(Link);

        if (TemplateDataModel != nameof(CardArticleModel))
        {
            throw new Exception($"{nameof(TemplateDataModel)} must be {nameof(CardArticleModel)} for the type {nameof(CardArticleModel)}.");
        }
    }
}
