using Blog.Builder.Exceptions;
using Blog.Builder.Interfaces;
using Blog.Builder.Interfaces.Builders;
using Blog.Builder.Models;
using Blog.Builder.Models.Templates;
using Microsoft.Extensions.Options;

namespace Blog.Builder.Services;

/// <inheritdoc/>
internal class WebsitePreparation : IWebsiteProcessor
{
    private readonly IPageProcessor _pageProcessor;
    private readonly ICardProcessor _cardProcessor;
    private readonly ISitemapBuilder _sitemapBuilder;
    private readonly IStaticAppConfigBuilder _staticAppConfigBuilder;
    private readonly AppSettings appSettings;
    private bool additionalCardsPrepared = false;
    private bool articlesPrepared = false;

    /// <summary>
    /// Besides DI, it creates and hold the basic information for the index pages. 
    /// See <seealso cref="LayoutIndexModel"/>.
    /// </summary>
    /// <param name="pageProcessor">The service that processes full pages (like index.html and privacy.html).</param>
    /// <param name="cardProcessor">The service that processes all cards.</param>
    /// <param name="sitemapBuilder">The service that builds the sitemap.xml.</param>
    /// <param name="staticAppConfigBuilder">The service that builds the staticwebapp.config.json.</param>
    /// <param name="options">The AppSettings</param>
    public WebsitePreparation(
                            IPageProcessor pageProcessor,
                            ICardProcessor cardProcessor,
                            ISitemapBuilder sitemapBuilder,
                            IStaticAppConfigBuilder staticAppConfigBuilder,
                            IOptions<AppSettings> options)
    {
        ExceptionHelpers.ThrowIfNull(pageProcessor);
        ExceptionHelpers.ThrowIfNull(cardProcessor);
        ExceptionHelpers.ThrowIfNull(sitemapBuilder);
        ExceptionHelpers.ThrowIfNull(staticAppConfigBuilder);
        ExceptionHelpers.ThrowIfNull(options);

        _pageProcessor = pageProcessor;
        _cardProcessor = cardProcessor;
        _sitemapBuilder = sitemapBuilder;
        _staticAppConfigBuilder = staticAppConfigBuilder;
        appSettings = options.Value;
    }

    /// <summary>
    /// Prepares the output folder located at <see cref="Globals.OutputFolderPath"/> by deleting it first 
    /// and then creating all the necessary folders again.
    /// It will also copy all the <see cref="Globals.WorkingJustCopyFolderName"/> directly to <see cref="Globals.OutputFolderPath"/>.
    /// </summary>
    private static void PrepareOutputFolders()
    {
        Directory.Delete(Globals.OutputFolderPath, true);
        Directory.CreateDirectory(Globals.OutputFolderPath);
        Directory.CreateDirectory(
            Path.Combine(Globals.OutputFolderPath, Globals.MediaFolderName)
        );
        Helpers.Copy(
            Path.Combine(Globals.WorkingFolderPath, Globals.WorkingJustCopyFolderName),
            Globals.OutputFolderPath
        );
    }

    /// <summary>
    /// Prepares all the standalone pages (like privacy.html).
    /// Standalones are scanned from <see cref="Globals.WorkingStandalonesFolderName"/>.
    /// </summary>
    private void PrepareStandalonePages()
    {

        var standalonesDirectory = Directory.GetDirectories(
            Path.Combine(Globals.WorkingFolderPath, Globals.WorkingStandalonesFolderName)
        );
        foreach (var directory in standalonesDirectory)
        {
            _pageProcessor.ProcessPage<LayoutStandaloneModel>(directory);
        }
    }

    /// <summary>
    /// Prepares all the article pages and the article cards for the index pages.
    /// Articles are scanned from <see cref="Globals.WorkingArticlesFolderName"/>.
    /// </summary>
    private void PrepareArticlePages()
    {
        if (!additionalCardsPrepared)
        {
            throw new Exception($"Method {nameof(PrepareArticlePages)} must be called after method {nameof(PrepareAdditionalCardsAsync)}.");
        }
        var articleDirectories = Directory.GetDirectories(
            Path.Combine(Globals.WorkingFolderPath, Globals.WorkingArticlesFolderName)
        );
        foreach (var directory in articleDirectories)
        {
            _pageProcessor.ProcessPage<LayoutArticleModel>(directory);
        }

        articlesPrepared = true;
    }

    /// <summary>
    /// Prepares all the additional cards for the index pages.
    /// Additional cards are scanned from <see cref="Globals.WorkingCardsFolderName"/>.
    /// </summary>
    private async Task PrepareAdditionalCardsAsync()
    {
        var cardsDirectory = Directory.GetDirectories(
            Path.Combine(Globals.WorkingFolderPath, Globals.WorkingCardsFolderName)
        );
        foreach (var directory in cardsDirectory)
        {
            await _cardProcessor.ProcessCardAsync(directory);
        }
        additionalCardsPrepared = true;
    }

    /// <summary>
    /// Prepares all the index pages (like index.html, index-page-2.html etc) 
    /// and copies it to <see cref="Globals.OutputFolderPath"/>.
    /// </summary>
    private void PrepareIndexPages()
    {

        if (!additionalCardsPrepared || !articlesPrepared)
        {
            throw new Exception($"Method {nameof(PrepareIndexPages)} must be called after methods {nameof(PrepareAdditionalCardsAsync)} and {nameof(PrepareArticlePages)}.");
        }
        var pageIndex = 0;
        var cardsNumber = _cardProcessor.GetCardsNumber(appSettings.CardsPerPage);
        var layoutIndexModel = new LayoutIndexModel(appSettings);
        layoutIndexModel.Paging.TotalCardsCount = cardsNumber;

        for (var i = 0; i < cardsNumber - 1; i++)
        {
            if (i % appSettings.CardsPerPage == 0 || i == cardsNumber - 1)
            {
                layoutIndexModel.Paging.CurrentPageIndex = pageIndex++;
                _pageProcessor.ProcessIndex(layoutIndexModel, appSettings.CardsPerPage);
            }
        }
    }


    /// <inheritdoc/>
    public async Task PrepareAsync()
    {
        PrepareOutputFolders();
        await PrepareAdditionalCardsAsync();
        PrepareStandalonePages();
        PrepareArticlePages();
        PrepareIndexPages();
        _sitemapBuilder.Build();
        _staticAppConfigBuilder.Build();
    }

    public void Dispose()
    {
        _pageProcessor.Dispose();
        _sitemapBuilder.Dispose();
        _cardProcessor.Dispose();
    }
}
