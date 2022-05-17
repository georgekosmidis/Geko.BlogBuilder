using Blog.Builder.Exceptions;

namespace Blog.Builder.Models.Templates;

/// <summary>
/// Used for the sitemap.xml (template-sitemap.cshtml)
/// </summary>
public record class LayoutSitemapModel : ModelBase
{

    /// <summary>
    /// Constructor.
    /// Sets the <see cref="ModelBase.TemplateDataModel"/> to nameof <see cref="LayoutSitemapModel"/>.
    /// </summary>
    /// <param name="appSettings">The appsettings.json model</param>
    public LayoutSitemapModel(AppSettings appSettings) : base(appSettings)
    {
        TemplateDataModel = nameof(LayoutSitemapModel);
    }

    /// <summary>
    /// A list of URLs which the sitemap.xml will include.
    /// </summary>
    public List<Url> Urls { get; set; } = new List<Url>();

    /// <summary>
    /// A helper method that adss a URL to the <see cref="Urls"/> list.
    /// </summary>
    /// <param name="relativeUrl">The relative URL of the page.</param>
    /// <param name="dateModified"> The last modified date.</param>
    public void Add(string relativeUrl, DateTime dateModified)
    {
        Urls.Add(new Url { RelativeUrl = relativeUrl, DateModified = dateModified });
    }

    /// <summary>
    /// Validates what this object knows and throws an exception if something is wrong.
    /// Check the <see cref="Validate"/> source code for the validations.
    /// </summary>
    public new void Validate()
    {
        base.Validate();

        ExceptionHelpers.ThrowIfNullOrEmpty(Urls);

        if (TemplateDataModel != nameof(LayoutSitemapModel))
        {
            throw new Exception($"{nameof(TemplateDataModel)} must be {nameof(LayoutSitemapModel)} for the type {nameof(LayoutSitemapModel)}.");
        }
    }
}

/// <summary>
/// An object that holds all urls for the sitemap.xml
/// </summary>
public record class Url
{
    /// <summary>
    /// The relative URL of the page.
    /// </summary>
    public string RelativeUrl { get; set; } = string.Empty;

    /// <summary>
    /// The last modified date.
    /// </summary>
    public DateTime DateModified { get; set; } = DateTime.MaxValue;
}