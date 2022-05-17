using Blog.Builder.Exceptions;

namespace Blog.Builder.Models.Templates;

/// <summary>
/// A card template model that displays an image.
/// </summary>
public record class CardImageModel : CardModelBase
{
    /// <summary>
    /// Ctor.
    /// </summary>
    /// <param name="appSettings">The appsettings.json model.</param>
    public CardImageModel(AppSettings appSettings) : base(appSettings)
    {
        TemplateDataModel = nameof(CardImageModel);
    }

    /// <summary>
    /// Validates what this object knows and throws an exception if something is wrong.
    /// Check the <see cref="Validate"/> source code for the validations.
    /// </summary>
    public new void Validate()
    {
        base.Validate();

        if (TemplateDataModel != nameof(CardImageModel))
        {
            throw new Exception($"{nameof(TemplateDataModel)} must be {nameof(CardImageModel)} for the type {nameof(CardImageModel)}.");
        }

        ExceptionHelpers.ThrowIfNullOrWhiteSpace(Title);
        ExceptionHelpers.ThrowIfNullOrWhiteSpace(Link);
        ExceptionHelpers.ThrowIfPathNotExists(ImageUrl);
    }
}