using Blog.Builder.Exceptions;
using Blog.Builder.Interfaces;
using Blog.Builder.Interfaces.Builders;
using Blog.Builder.Interfaces.RazorEngineServices;
using Blog.Builder.Models;
using Blog.Builder.Models.Builders;
using Blog.Builder.Models.Templates;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using System.Text.RegularExpressions;

namespace Blog.Builder.Services.Builders;

/// <inheritdoc/>
internal class PageBuilder : IPageBuilder
{
    private readonly IRazorEngineWrapperService _templateEngine;
    private readonly ICardProcessor _cardPreparation;
    private readonly ILogger _logger;
    private readonly AppSettings appSettings;

    public PageBuilder(IRazorEngineWrapperService templateService, ICardProcessor cardPreparation, ILogger logger, IOptions<AppSettings> options)
    {
        ArgumentNullException.ThrowIfNull(templateService);
        ArgumentNullException.ThrowIfNull(cardPreparation);
        ArgumentNullException.ThrowIfNull(options);

        _templateEngine = templateService;
        _cardPreparation = cardPreparation;
        _logger = logger;
        appSettings = options.Value;
    }

    /// <inheritdoc/>
    public string BuildInternalLayoutForPages<T>(T pageData, string bodyHtml, IEnumerable<string> rightColumnCards) where T : LayoutModelBase
    {
        ExceptionHelpers.ThrowIfNull(pageData);
        ExceptionHelpers.ThrowIfNullOrWhiteSpace(bodyHtml);

        pageData.RightColumnCards = rightColumnCards;
        pageData.Body = bodyHtml;
        pageData.TemplateDataModel = typeof(T).Name;
        var innerPartResult = Build(pageData);

        ExceptionHelpers.ThrowIfNull(pageData);

        _logger.Log($"Processing of {pageData.RelativeUrl} done!");
        return innerPartResult.FinalHtml;
    }

    /// <inheritdoc/>
    public string BuildInternalLayoutForIndex<T>(T pageData, IEnumerable<string> bodyCards, IEnumerable<string> rightColumnCards) where T : LayoutModelBase
    {
        ExceptionHelpers.ThrowIfNull(pageData);
        ExceptionHelpers.ThrowIfNullOrEmpty(bodyCards);

        //First prepare the body of the page
        // (Right column is in the inner layouts, not the main layout)
        pageData.RightColumnCards = rightColumnCards;
        pageData.CardsHtml = bodyCards;
        pageData.TemplateDataModel = typeof(T).Name;
        var innerPartResult = Build(pageData);

        ExceptionHelpers.ThrowIfNull(pageData);

        _logger.Log($"Processing of {pageData.RelativeUrl}, page {pageData.Paging.CurrentPageIndex + 1} done!");
        return innerPartResult.FinalHtml;
    }

    /// <inheritdoc/>
    private PageBuilderResult Build<T>(T pageData) where T : LayoutModelBase
    {
        ExceptionHelpers.ThrowIfNull(pageData);
        pageData.Validate();

        var finalHtml = _templateEngine.RunCompile(pageData);

        //Add CreatorID
        var linkParser = new Regex("http(s?):\\/\\/([^\"'\\s<>]*)(microsoft\\.com|azure\\.cn|azure\\.com|msdn\\.com)([^\"'\\s<>)]*)", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        var urlsToChange = new Dictionary<string, string>();
        foreach (Match m in linkParser.Matches(finalHtml))
        {
            if (m.Value.Contains("mvp.microsoft.com") || m.Value.Contains($"WT.mc_id={appSettings.MicrosoftCreatorID}"))
            {
                continue;
            }
            urlsToChange[m.Value] = QueryHelpers.AddQueryString(m.Value, "WT.mc_id", appSettings.MicrosoftCreatorID);

        }
        foreach (var (oldUrl, newUrl) in urlsToChange.Distinct())
        {
            finalHtml = finalHtml.Replace(oldUrl, newUrl);
        }

        return new PageBuilderResult
        {
            FinalHtml = finalHtml,
            DateModified = pageData.DateModified,
            RelativeUrl = pageData.RelativeUrl
        };
    }

    /// <inheritdoc/>
    public PageBuilderResult BuildMainLayout<T>(T pageData, string bodyHtml) where T : LayoutModelBase
    {
        pageData.Body = bodyHtml;
        var mainBuilderResult = Build(pageData as LayoutModelBase);

        //At the end, add a card for this article
        //todo: should that be here?
        if (pageData.TemplateDataModel == nameof(LayoutArticleModel))
        {
            var articleData = pageData as LayoutArticleModel;
            ExceptionHelpers.ThrowIfNull(articleData);

            var footer = articleData.DateModifiedText == articleData.DatePublishedText ? $"Published {articleData.DatePublishedText}" : $"Modified {articleData.DateModifiedText}";

            var cardData = new CardArticleModel(appSettings)
            {
                TemplateDataModel = nameof(CardArticleModel),
                Title = articleData.Title,
                Description = articleData.Description,
                ImageUrl = articleData.RelativeImageUrl,
                Link = pageData.RelativeUrl,
                LinkTarget = "_top",
                IsSticky = false,
                Position = -1,
                Footer = footer
            };
            _cardPreparation.ProcessArticleCard(cardData, pageData.DatePublished);
        }

        return mainBuilderResult;
    }

    public void Dispose()
    {
        _templateEngine.Dispose();
    }
}