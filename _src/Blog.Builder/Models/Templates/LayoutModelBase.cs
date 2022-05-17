using Blog.Builder.Exceptions;
using System.Text.RegularExpressions;

namespace Blog.Builder.Models.Templates;

/// <summary>
/// Used for the main template, the layout (template-layout.cshtml).
/// Inherits all members of <see cref="ModelBase"/>.
/// </summary>
public record class LayoutModelBase : ModelBase
{

    /// <summary>
    /// Ctor.
    /// </summary>
    /// <param name="appSettings">The appsettings.json model</param>
    public LayoutModelBase(AppSettings appSettings) : base(appSettings)
    {
        TemplateDataModel = nameof(LayoutModelBase);

        LastBuild = DateTime.UtcNow;
        RightColumnCards = new List<string>();
        CardsHtml = new List<string>();
        Paging = new Paging();
        RelativeUrl = "/";
        Title = appSettings.BlogTitle;
        Description = appSettings.BlogDescription;
        Tags = appSettings.BlogTags;
        Sections = new List<string>();
        ExtraHeaders = new List<string>();
        DateExpires = DateTime.MaxValue;

    }

    /// <summary>
    /// Last Build UTC
    /// </summary>
    public DateTime LastBuild { get; }

    /// <summary>
    /// The github repo URL of this article.
    /// </summary>
    public string? GithubCurrentPageUrl { get; set; }

    /// <summary>
    /// The HTML for the right column.
    /// Default is empty.
    /// </summary>
    public IEnumerable<string> RightColumnCards { get; set; }

    /// <summary>
    /// The HTML of the cards to be parsed
    /// </summary>
    public IEnumerable<string> CardsHtml { get; set; }

    /// <summary>
    /// The paging information for this layout. 
    /// Currently it is used only by index pages.
    /// </summary>
    public Paging Paging { get; set; }

    /// <summary>
    /// The relative URL of the current page.
    /// </summary>
    public string RelativeUrl { get; set; }


    /// <summary>
    /// The title of the current page.
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// The tags of the current page.
    /// </summary>
    public IEnumerable<string> Tags { get; }

    /// <summary>
    /// A text representation for the <see cref="Tags"/> list.
    /// </summary>
    public string TagsText => string.Join(", ", Tags);

    /// <summary>
    /// The section list of the current page.
    /// </summary>
    public IEnumerable<string> Sections { get; }

    /// <summary>
    /// A text representation for the <see cref="Sections"/> list.
    /// </summary>
    public string SectionsText => string.Join(", ", Sections);

    /// <summary>
    /// Any list of extra headers to be included in the current page.
    /// </summary>
    public IEnumerable<string> ExtraHeaders { get; }

    /// <summary>
    /// A text representation for the <see cref="ExtraHeaders"/> list.
    /// </summary>
    public string ExtraHeadersText => string.Join(", ", ExtraHeaders);

    /// <summary>
    /// An HTML description of the current page.
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// The date this page was first published.
    /// </summary>
    public DateTime DatePublished { get; set; }

    /// <summary>
    /// The date this page was last modified.
    /// </summary>
    public DateTime DateModified { get; set; }

    /// <summary>
    /// The date the information on this page expires.
    /// Default is <see cref="DateTime.MaxValue"/>.
    /// </summary>
    public DateTime DateExpires { get; }

    /// <summary>
    /// The path to the main image of this page.
    /// </summary>
    public string? RelativeImageUrl { get; set; }

    /// <summary>
    /// The HTML body of this page.
    /// </summary>
    public string? Body { get; set; }

    /// <summary>
    /// A calculated description of this page in plain text.
    /// Uses regular expressions to remove all tags from the <see cref="Description"/>.
    /// </summary>
    public string PlainTextDescription
    {
        get
        {
            string? result = Regex.Replace(Description, "<.*?>", " ", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Singleline);
            return result.Trim();
        }
    }

    /// <summary>
    /// Validates what this object knows and throws an exception if something is wrong.
    /// Check the <see cref="Validate"/> source code for the validations.
    /// </summary>
    public new void Validate()
    {
        base.Validate();

        ExceptionHelpers.ThrowIfNull(DateModified);
        ExceptionHelpers.ThrowIfNull(DatePublished);
        ExceptionHelpers.ThrowIfNull(DateExpires);
        ExceptionHelpers.ThrowIfNullOrEmpty(Tags);
        ExceptionHelpers.ThrowIfNull(Sections);
        ExceptionHelpers.ThrowIfNull(ExtraHeaders);

        ExceptionHelpers.ThrowIfNullOrWhiteSpace(Description);
        ExceptionHelpers.ThrowIfNullOrWhiteSpace(PlainTextDescription);
        ExceptionHelpers.ThrowIfNullOrWhiteSpace(Title);
        ExceptionHelpers.ThrowIfNullOrWhiteSpace(RelativeUrl);

    }

}
