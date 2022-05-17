namespace Blog.Builder;

/// <summary>
/// Constants and variables used through out the system.
/// </summary>
public static class Globals
{
    /// <summary>
    /// The working folder, the folder that contains standalones, articles etc.
    /// </summary>
    public static string WorkingFolderPath { get; set; } = default!;

    /// <summary>
    /// The folder that everything will be copied at the end of the process.
    /// </summary>
    public static string OutputFolderPath { get; set; } = default!;

    /// <summary>
    /// The name of the media folder in the output directory.
    /// </summary>
    public const string MediaFolderName = "media";

    /// <summary>
    /// The name of the folder that holds all tempaltes.
    /// </summary>
    public const string WorkingTemplatesFolderName = "templates";

    /// <summary>
    /// The name of the folder that holds all items to be copied directly without any process.
    /// </summary>
    public const string WorkingJustCopyFolderName = "justcopyme";

    /// <summary>
    /// The name of the folder where all the articles live.
    /// </summary>
    public const string WorkingArticlesFolderName = "articles";

    /// <summary>
    /// The name of the folder for the standalones (like privacy.html).
    /// </summary>
    public const string WorkingStandalonesFolderName = "standalones";

    /// <summary>
    /// The name of the folder that holds all additional cards (besides the article cards).
    /// </summary>
    public const string WorkingCardsFolderName = "cards";

    /// <summary>
    /// The name of the folder that holds information about the events, along with its card.json.
    /// It's a static and not constant because <see cref="Path.DirectorySeparatorChar"/> is a static and not const!
    /// </summary>
    public static readonly string WorkingEventsFolderName = $"{WorkingCardsFolderName}{Path.DirectorySeparatorChar}events";

    /// <summary>
    /// The filename of the main template, the layout.
    /// </summary>
    public const string TemplateMainFilename = "template-layout.cshtml";

    /// <summary>
    /// The filename of the index page.
    /// </summary>
    public const string TemplateIndexFilename = "template-index.cshtml";

    /// <summary>
    /// The filename of the article template
    /// </summary>
    public const string TemplateArticleFilename = "template-article.cshtml";

    /// <summary>
    /// The filename of the sitemap template.
    /// </summary>
    public const string TemplateSitemapFilename = "template-sitemap.cshtml";

    /// <summary>
    /// The filename of the template for the standalones.
    /// </summary>
    public const string TemplateStandaloneFilename = "template-standalone.cshtml";

    /// <summary>
    /// The filename of the template for the article cards.
    /// </summary>
    public const string TemplateCardArticleFilename = "template-card-article.cshtml";

    /// <summary>
    /// The filename of the template from the image cards.
    /// </summary>
    public const string TemplateCardImageFilename = "template-card-image.cshtml";

    /// <summary>
    /// The filename of the template for the search.
    /// </summary>
    public const string TemplateCardSearchFilename = "template-card-search.cshtml";

    /// <summary>
    /// The filename of the template for the calendar events.
    /// </summary>
    public const string TemplateCardCalendarEventsFilename = "template-card-calendar-events.cshtml";

    /// <summary>
    /// The filename of the json file that describes an event.
    /// </summary>
    public const string EventJsonFilename = "event.json";

    /// <summary>
    /// THe filename of the json file that describes a card.
    /// </summary>
    public const string CardJsonFilename = "card.json";

    /// <summary>
    /// The filename of the json file that describes a content (article or standalone).
    /// </summary>
    public const string ContentJsonFilename = "content.json";

    /// <summary>
    /// The filename of the HTML for an article or a standalone
    /// </summary>
    public const string ContentHtmlFilename = "content.html";

    /// <summary>
    /// The filename of the google sitemap.
    /// </summary>
    public const string GoogleSitemap = "sitemap.xml";

    /// <summary>
    /// The name of the appsettings file.
    /// </summary>
    public const string AppSettingsFilename = "appsettings.json";

    /// <summary>
    /// The name of the Static Web App config.
    /// For more information, please visit: https://docs.microsoft.com/en-us/azure/static-web-apps/configuration
    /// </summary>
    public const string StaticWebAppFilename = "staticwebapp.config.json";

}
