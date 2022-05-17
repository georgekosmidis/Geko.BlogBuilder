using Blog.Builder.Exceptions;
using Blog.Builder.Interfaces.Builders;
using Blog.Builder.Models.Builders;
using Newtonsoft.Json;

namespace Blog.Builder.Services.Builders;

/// <summary>
/// A builder that supports part of the routes for staticwebapp.config.json
/// Check https://docs.microsoft.com/en-us/azure/static-web-apps/configuration for more information
/// </summary>
internal class StaticAppConfigBuilder : IStaticAppConfigBuilder
{
    public static readonly List<string> Routes = new();

    /// <inheritdoc/>
    public StaticAppConfigBuilder()
    {

    }

    /// <summary>
    /// Registers the routes to redirect old wordpress routes.
    /// </summary>
    /// <param name="relativeUrl">The new relative URL. Old wordpress URLs will be build out of this.</param>
    /// <param name="datePublished">The date an article is published is used by wordpress in the format YYYY/MM/DD.</param>
    public void Add(string relativeUrl, DateTime datePublished)
    {
        var filenameWithoutExtension = Path.GetFileNameWithoutExtension(relativeUrl);
        ExceptionHelpers.ThrowIfNullOrWhiteSpace(filenameWithoutExtension);

        var filename = Path.GetFileName(relativeUrl);
        ExceptionHelpers.ThrowIfNullOrWhiteSpace(filename);

        //add route for the format /YYYY/MM/DD/slang-title/
        var routeDT = new RouteRedirect()
        {
            Route = $"/{datePublished.Year}/{datePublished.Month:00}/{datePublished.Day:00}/{filenameWithoutExtension}*",
            Redirect = relativeUrl,
            StatusCode = 301
        };
        Routes.Add(JsonConvert.SerializeObject(routeDT));

        //add route for the format /slang-title/
        var routeFolder = new RouteRedirect()
        {
            Route = $"/{filenameWithoutExtension}*",
            Redirect = relativeUrl,
            StatusCode = 301
        };
        Routes.Add(JsonConvert.SerializeObject(routeFolder));
    }

    public void Build()
    {

        //add redirect for HTML
        var routeRoot = new RouteRedirect()
        {
            Route = $"/index.html*",
            Redirect = "/",
            StatusCode = 301
        };
        Routes.Add(JsonConvert.SerializeObject(routeRoot));

        //add cache for media
        var routeMedia = new RouteHeaders()
        {
            Route = $"/{Globals.MediaFolderName}*",
            Headers = new Dictionary<string, string>
            {
               { "cache-control", "must-revalidate, max-age=2628000" }//2628000 seconds = 1 month
            }
        };
        Routes.Add(JsonConvert.SerializeObject(routeMedia));

        //add cache for HTML
        var routeHtml = new RouteHeaders()
        {
            Route = $"/*.html",
            Headers = new Dictionary<string, string>
            {
               { "cache-control", "must-revalidate, max-age=3600" }//86400 seconds = 1 hour
            }
        };
        Routes.Add(JsonConvert.SerializeObject(routeHtml));

        //Save the routes
        var staticWebAppConfigContent = $"{{\"routes\":[{string.Join(',', Routes.ToArray())}]}}";
        var staticWebAppConfigFile = Path.Combine(Globals.OutputFolderPath, Globals.StaticWebAppFilename);

        File.WriteAllText(staticWebAppConfigFile, staticWebAppConfigContent);

    }
}
