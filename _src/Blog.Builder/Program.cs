using Blog.Builder;
using Blog.Builder.Interfaces;
using Blog.Builder.Interfaces.Builders;
using Blog.Builder.Interfaces.Crawlers;
using Blog.Builder.Interfaces.RazorEngineServices;
using Blog.Builder.Models;
using Blog.Builder.Services;
using Blog.Builder.Services.Builders;
using Blog.Builder.Services.Crawlers;
using Blog.Builder.Services.RazorEngineServices;
using Geko.HttpClientService.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RazorEngine.Templating;
using WebMarkupMin.Core;

var serviceProvider = new ServiceCollection()
        .AddLogging()
        .AddHttpClientService()
        .AddSingleton<ILogger, Logger>()
        .AddSingleton<IRazorEngineWrapperService, RazorEngineWrapperService>()
        .AddSingleton<ITemplateManager, TemplateManager>()
        .AddSingleton<ITemplateProvider, TemplateProvider>()
        .AddSingleton<ISitemapBuilder, SitemapBuilder>()
        .AddSingleton<IPageBuilder, PageBuilder>()
        .AddSingleton<IPageProcessor, PageProcessor>()
        .AddSingleton<ICardBuilder, CardBuilder>()
        .AddSingleton<ICardProcessor, CardProcessor>()
        .AddSingleton<IWebsiteProcessor, WebsitePreparation>()
        .AddSingleton<IMeetupEventCrawler, MeetupEventCrawler>()
        .AddSingleton<IFileEventCrawler, FileEventCrawler>()
        .AddSingleton<IStaticAppConfigBuilder, StaticAppConfigBuilder>()
        .AddSingleton<IMarkupMinifier>(provider =>
        {
            var settings = new HtmlMinificationSettings()
            {
                AttributeQuotesRemovalMode = HtmlAttributeQuotesRemovalMode.Html5,
                CollapseBooleanAttributes = true,
                MinifyEmbeddedCssCode = true,
                MinifyEmbeddedJsCode = true,
                RemoveHtmlComments = true,
                RemoveOptionalEndTags = false,
                WhitespaceMinificationMode = WhitespaceMinificationMode.Safe
            };
            return new HtmlMinifier(settings);
        })
        .AddOptions()
        .Configure<AppSettings>(
            new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(Globals.AppSettingsFilename, optional: false)
                .AddEnvironmentVariables()
                .Build()
        )
        .BuildServiceProvider();

if (args.Length > 0)
{
    if (!args.Contains("--workables") || !args.Contains("--output"))
    {
        throw new ArgumentException("Two arguments are required: '--workables' and '--output'!");
    }

    if (args[0] == "--workables")
    {
        Globals.WorkingFolderPath = args[1];
        Globals.OutputFolderPath = args[3];
    }
    else
    {
        Globals.OutputFolderPath = args[1];
        Globals.WorkingFolderPath = args[3];
    }
}

//todo: clean template models, it seems they are too complicated
//todo: bigger images on tap, is it possible?
//todo: add commenting system
//todo: custom 404, etc

var websitePreparation = serviceProvider.GetService<IWebsiteProcessor>() ?? throw new NullReferenceException(nameof(IWebsiteProcessor));
await websitePreparation.PrepareAsync();
websitePreparation.Dispose();