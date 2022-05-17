using Blog.Builder.Exceptions;
using System.Diagnostics.CodeAnalysis;

namespace Blog.Builder.Models.Templates;

/// <summary>
/// The base model of all tempalte models.
/// </summary>
public record class ModelBase
{
    /// <summary>
    /// Ctor.
    /// </summary>
    /// <param name="appSettings">The appsettings.json model</param>
    public ModelBase(AppSettings appSettings)
    {
        TemplateDataModel = nameof(ModelBase);
        TwitterHandle = appSettings.TwitterHandle;
        GithubRepoUrl = appSettings.GithubRepoUrl;
        AuthorPersonalPage = appSettings.AuthorPersonalPage;
        AuthorName = appSettings.AuthorName;
        BlogTitle = appSettings.BlogTitle;
        BlogDescription = appSettings.BlogDescription;
    }

    /// <summary>
    /// Property that holds the current template model name.
    /// </summary>
    public string TemplateDataModel { get; set; }

    /// <summary>
    /// The twitter handle of the author (or blog)
    /// </summary>
    public string TwitterHandle { get; } = string.Empty;

    /// <summary>
    /// The Github repo URL
    /// </summary>
    public string GithubRepoUrl { get; } = default!;

    /// <summary>
    /// The author's personal page
    /// </summary>
    public string AuthorPersonalPage { get; } = string.Empty;

    /// <summary>
    /// The author name of the blog posts
    /// </summary>
    public string AuthorName { get; } = string.Empty;

    /// <summary>
    /// The title of the blog.
    /// </summary>
    public string BlogTitle { get; } = default!;

    /// <summary>
    /// The URL of the blog
    /// </summary>
    public string BlogUrl { get; set; } = default!;

    /// <summary>
    /// The description of the blog.
    /// </summary>
    public string BlogDescription { get; } = default!;

    private static readonly Guid nonce = Guid.NewGuid();
    /// <summary>
    /// A static Guid as the cryptographic nonce attribute.
    /// Since the build is static a new nonce will not produced in every visit but in every build.
    /// </summary>
    /// <remarks>
    /// todo: find a way to produce a new nonce for every page load, which means it cannot be part of the build,
    ///       but maybe part of the host?
    /// </remarks>
    [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "RazorEngine wants it to be an instance member.")]
    public Guid Nonce => nonce;

    /// <summary>
    /// Validates what this object knows and throws an exception if something is wrong.
    /// Check the <see cref="Validate"/> source code for the validations.
    /// </summary>
    public void Validate()
    {
        ExceptionHelpers.ThrowIfNull(Nonce);
        ExceptionHelpers.ThrowIfNullOrWhiteSpace(TemplateDataModel);
    }
}
