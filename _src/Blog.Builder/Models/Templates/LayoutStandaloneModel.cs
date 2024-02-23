using Blog.Builder.Exceptions;

namespace Blog.Builder.Models.Templates;

/// <summary>
/// Used for the standalones, e.g. privacy.html (template-standalone.cshtml).
/// Inherits all members of <see cref="LayoutModelBase"/>.
/// </summary>
public record class LayoutStandaloneModel : LayoutModelBase
{
    /// <summary>
    /// Ctor
    /// </summary>
    /// <param name="appSettings">The appsettings.json model</param>
    public LayoutStandaloneModel(AppSettings appSettings) : base(appSettings)
    {
        TemplateDataModel = nameof(LayoutStandaloneModel);
    }

    /// <summary>
    /// The calculated path for the small version of the image.
    /// The smaller version is automatically created from <see cref="Services.PageProcessor.ProcessPage{T}(string)"/>.
    /// </summary>
    public string? RelativeImageUrlSmall => RelativeImageUrl is null
                ? null
                : (Path.GetDirectoryName(RelativeImageUrl) ?? string.Empty).Replace("\\", "/")
                    + "/"
                    + Path.GetFileNameWithoutExtension(RelativeImageUrl) + "-small" + Path.GetExtension(RelativeImageUrl);

    /// <summary>
    /// Validates what this object knows and throws an exception if something is wrong.
    /// Check the <see cref="Validate"/> source code for the validations.
    /// </summary>
    public new void Validate()
    {
        base.Validate();

        ExceptionHelpers.ThrowIfNullOrWhiteSpace(Body);

        if (TemplateDataModel != nameof(LayoutStandaloneModel))
        {
            throw new Exception($"{nameof(TemplateDataModel)} must be {nameof(LayoutStandaloneModel)} for the type {nameof(LayoutStandaloneModel)}.");
        }
    }
}
