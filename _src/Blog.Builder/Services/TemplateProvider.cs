using Blog.Builder.Exceptions;
using Blog.Builder.Interfaces;
using Blog.Builder.Models;
using Blog.Builder.Models.Templates;
using Microsoft.Extensions.Options;

namespace Blog.Builder.Services;

/// <inheritdoc/>
internal class TemplateProvider : ITemplateProvider
{
    /// <summary>
    /// A dictionary with key the name of a tempalte model (e.g. <see cref="LayoutArticleModel"/>)
    /// and value the html of proper template. 
    /// </summary>
    private readonly Dictionary<string, string> Templates = new();

    /// <summary>
    /// Besides DI, it registers all template htmls in a dictionary with key being the name of the model for that dictionary.
    /// </summary>
    /// <param name="options">The options that hold the app settings.</param>
    public TemplateProvider(IOptions<AppSettings> options)
    {
        ArgumentNullException.ThrowIfNull(options);

        var appSettings = options.Value;
        var workingTemplatesFolder = Path.Combine(Globals.WorkingFolderPath, Globals.WorkingTemplatesFolderName);

        Templates = new Dictionary<string, string>()
        {
            { nameof(LayoutIndexModel), Path.Combine(workingTemplatesFolder, Globals.TemplateIndexFilename) },
            { nameof(LayoutModelBase), Path.Combine(workingTemplatesFolder, Globals.TemplateMainFilename) },
            { nameof(LayoutArticleModel), Path.Combine(workingTemplatesFolder, Globals.TemplateArticleFilename) },
            { nameof(LayoutStandaloneModel), Path.Combine(workingTemplatesFolder, Globals.TemplateStandaloneFilename) },
            { nameof(LayoutSitemapModel), Path.Combine(workingTemplatesFolder, Globals.TemplateSitemapFilename) },
            { nameof(CardArticleModel), Path.Combine(workingTemplatesFolder, Globals.TemplateCardArticleFilename) },
            { nameof(CardImageModel), Path.Combine(workingTemplatesFolder, Globals.TemplateCardImageFilename) },
            { nameof(CardSearchModel),Path.Combine(workingTemplatesFolder, Globals.TemplateCardSearchFilename) },
            { nameof(CardCalendarEventsModel), Path.Combine(workingTemplatesFolder, Globals.TemplateCardCalendarEventsFilename) },
        };

        foreach ((var model, var path) in Templates)
        {
            ExceptionHelpers.ThrowIfPathNotExists(path, model);
        }
    }

    /// <inheritdoc/>
    public string GetHtml<T>()
    {
        return File.ReadAllText(GetPath<T>());
    }

    /// <inheritdoc/>
    public string GetHtml(string nameOfType)
    {
        return File.ReadAllText(GetPath(nameOfType));
    }

    /// <inheritdoc/>
    public string GetPath<T>()
    {
        var path = Templates.First(x => x.Key == typeof(T).Name);
        return path.Value;
    }

    /// <inheritdoc/>
    public string GetPath(string nameOfType)
    {
        var path = Templates.First(x => x.Key == nameOfType);
        return path.Value;
    }
}
