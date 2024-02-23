using Blog.Builder.Exceptions;
using Blog.Builder.Interfaces;
using Blog.Builder.Interfaces.Builders;
using Blog.Builder.Models;
using Blog.Builder.Models.Builders;
using Blog.Builder.Models.Templates;
using Markdig;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SixLabors.ImageSharp;
using WebMarkupMin.Core;

namespace Blog.Builder.Services;

/// <inheritdoc/>
internal class PageProcessor : IPageProcessor
{
    private readonly ISitemapBuilder _sitemapBuilder;
    private readonly IMarkupMinifier _markupMinifier;
    private readonly IPageBuilder _pageBuilder;
    private readonly ICardProcessor _cardProcessor;
    private readonly IStaticAppConfigBuilder _staticAppConfigBuilder;
    private readonly AppSettings appSettings;

    public PageProcessor(ISitemapBuilder sitemapBuilder,
                        IMarkupMinifier markupMinifier,
                        IPageBuilder pageBuilder,
                        ICardProcessor cardProcessor,
                        IStaticAppConfigBuilder staticAppConfigBuilder,
                        IOptions<AppSettings> options)
    {
        ArgumentNullException.ThrowIfNull(sitemapBuilder);
        ArgumentNullException.ThrowIfNull(markupMinifier);
        ArgumentNullException.ThrowIfNull(pageBuilder);
        ArgumentNullException.ThrowIfNull(cardProcessor);
        ArgumentNullException.ThrowIfNull(staticAppConfigBuilder);
        ArgumentNullException.ThrowIfNull(options);

        _sitemapBuilder = sitemapBuilder;
        _markupMinifier = markupMinifier;
        _pageBuilder = pageBuilder;
        _cardProcessor = cardProcessor;
        _staticAppConfigBuilder = staticAppConfigBuilder;
        appSettings = options.Value;
    }

    //todo: introduce caching
    private T GetPageModelData<T>(string jsonPath) where T : LayoutModelBase
    {
        ExceptionHelpers.ThrowIfPathNotExists(jsonPath);

        var model = Activator.CreateInstance(typeof(T), new object[] { appSettings });
        ExceptionHelpers.ThrowIfNull(model);

        string json = File.ReadAllText(jsonPath);
        var serializerSettings = new JsonSerializerSettings { ObjectCreationHandling = ObjectCreationHandling.Replace };
        JsonConvert.PopulateObject(json, model, serializerSettings);
        ExceptionHelpers.ThrowIfNull(model);

        return (model as T)!;
    }

    private PageBuilderResult GetBuilderResult<T>(string directory) where T : LayoutModelBase
    {
        ExceptionHelpers.ThrowIfPathNotExists(directory);

        //read json and html from the folder
        string? jsonFileContent = Path.Combine(directory, Globals.ContentJsonFilename);

        T? pageData = GetPageModelData<T>(jsonFileContent);
        ExceptionHelpers.ThrowIfNull(pageData);

        if(File.Exists(Path.Combine(directory, Globals.ContentMdFilename)))
        {
            string? pageMd = File.ReadAllText(Path.Combine(directory, Globals.ContentMdFilename));

            var pipeline = new MarkdownPipelineBuilder()
                .UseAutoIdentifiers()
                .Build();
            string? htmlText = Markdown.ToHtml(pageMd, pipeline);
            File.WriteAllText(Path.Combine(directory, Globals.ContentHtmlFilename), htmlText);
        }
        string? pageHtml = File.ReadAllText(Path.Combine(directory, Globals.ContentHtmlFilename));
        ExceptionHelpers.ThrowIfNullOrWhiteSpace(pageHtml);

        //add azure static web app routes
        _staticAppConfigBuilder.Add(pageData.RelativeUrl, pageData.DatePublished);

        //add the GitHub repo
        string? articleFolderName = Path.GetFileName(directory.Trim(Path.DirectorySeparatorChar));
        pageData.GithubCurrentPageUrl = $"{appSettings.GithubRepoUrl}/tree/main/workables/{Globals.WorkingArticlesFolderName}/{articleFolderName}/{Globals.ContentHtmlFilename}";

        //get the right column cards, if any
        IEnumerable<string>? rightColumnCards = _cardProcessor.GetRightColumnCardsHtml();

        //get the inner layout build
        string? internalHtml = _pageBuilder.BuildInternalLayoutForPages(pageData, pageHtml, rightColumnCards);

        //get the outer layout build
        PageBuilderResult? builderResult = _pageBuilder.BuildMainLayout(pageData, internalHtml);

        //done!
        return builderResult;
    }

    private PageBuilderResult GetBuilderResult<T>(T pageData, IEnumerable<string> pageCards) where T : LayoutModelBase
    {
        ExceptionHelpers.ThrowIfNull(pageData);
        ExceptionHelpers.ThrowIfNullOrEmpty(pageCards);

        //get the right column cards, if any
        IEnumerable<string>? rightColumnCards = _cardProcessor.GetRightColumnCardsHtml();

        //get the inner layout build
        string? internalHtml = _pageBuilder.BuildInternalLayoutForIndex(pageData, pageCards, rightColumnCards);

        //get the outer layout build
        PageBuilderResult? builderResult = _pageBuilder.BuildMainLayout(pageData, internalHtml);

        //done!
        return builderResult;
    }

    /// <inheritdoc/>
    public void ProcessPage<T>(string directory) where T : LayoutModelBase
    {
        ExceptionHelpers.ThrowIfPathNotExists(directory);

        //get the builder result
        PageBuilderResult? builderResult = GetBuilderResult<T>(directory);

        //minify and save page
        MarkupMinificationResult? minifier = _markupMinifier.Minify(builderResult.FinalHtml);
        if (minifier.Errors.Count > 0)
        {
            //todo: add all errors
            throw new Exception($"Minification failed with at least one error: " +
                $"{Environment.NewLine}{minifier.Errors.First().Message}" +
                $"{Environment.NewLine}{minifier.Errors.First().SourceFragment}");
        }
        ExceptionHelpers.ThrowIfNullOrWhiteSpace(minifier.MinifiedContent);
        var savingPath = Path.Combine(Globals.OutputFolderPath, Path.GetFileName(builderResult.RelativeUrl));
        File.WriteAllText(savingPath, minifier.MinifiedContent);

        //copy all media associated with this page
        if (Directory.Exists(Path.Combine(directory, Globals.MediaFolderName)))
        {
            //copy original
            Helpers.Copy(
                    Path.Combine(directory, Globals.MediaFolderName),
                    Path.Combine(Globals.OutputFolderPath, Globals.MediaFolderName)
            );

            //create and copy a smaller version
            foreach (string? file in Directory.GetFiles(Path.Combine(directory, Globals.MediaFolderName)))
            {
                string? ext = Path.GetExtension(file);
                string? name = Path.GetFileNameWithoutExtension(file);

                Helpers.ResizeImage(file,
                    Path.Combine(Globals.OutputFolderPath, Globals.MediaFolderName, name + "-small" + ext),
                    new Size(500, 10000)
                );//stop at 500 width, who cares about height :)
            }
        }

        //add the page to the sitemap builder
        _sitemapBuilder.Add(builderResult.RelativeUrl, builderResult.DateModified);
    }

    /// <inheritdoc/>
    public void ProcessIndex(LayoutIndexModel pageData, int cardsPerPage)
    {
        ArgumentNullException.ThrowIfNull(pageData);

        //Get all the cards html for the main page
        IEnumerable<string> cards = _cardProcessor.GetBodyCardsHtml(pageData.Paging.CurrentPageIndex, cardsPerPage);

        //build the page
        PageBuilderResult? builderResult = GetBuilderResult(pageData, cards);

        var minifier = _markupMinifier.Minify(builderResult.FinalHtml);
        if (minifier.Errors.Count > 0)
        {
            throw new Exception($"Minification failed with at least one error:" +
                $"{Environment.NewLine}{minifier.Errors.First().Message}" +
                $"{Environment.NewLine}{minifier.Errors.First().SourceFragment}");
        }
        ExceptionHelpers.ThrowIfNullOrWhiteSpace(minifier.MinifiedContent);

        //save it
        var indexPageNumber = pageData.Paging.CurrentPageIndex == 0 ? string.Empty : "-page-" + (pageData.Paging.CurrentPageIndex + 1);
        var savingPath = Path.Combine(Globals.OutputFolderPath, $"index{indexPageNumber}.html");
        File.WriteAllText(savingPath, minifier.MinifiedContent);

        //add it to sitemap.xml
        pageData.RelativeUrl = $"/index{indexPageNumber}.html";
        _sitemapBuilder.Add(string.IsNullOrWhiteSpace(indexPageNumber) ? "/" : $"/index{indexPageNumber}.html", builderResult.DateModified);
    }

    public void Dispose()
    {
        _sitemapBuilder.Dispose();
        _pageBuilder.Dispose();
        _cardProcessor.Dispose();
    }
}
