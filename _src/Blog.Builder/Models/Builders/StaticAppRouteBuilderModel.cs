using Newtonsoft.Json;

namespace Blog.Builder.Models.Builders;


/// <summary>
/// The base class for each route in the staticwebapp.config.json.
/// Read more at: https://docs.microsoft.com/en-us/azure/static-web-apps/configuration
/// </summary>
public abstract class RouteBase
{
    /// <summary>
    /// The incoming route.
    /// </summary>
    [JsonProperty("route")]
    public string Route { get; set; } = default!;
}

/// <summary>
/// Adds headers to the specific incoming route.
/// </summary>
public class RouteHeaders : RouteBase
{
    /// <summary>
    /// A dictionary of headers, in the format "key: value".
    /// </summary>
    [JsonProperty("headers")]
    public Dictionary<string, string> Headers { get; set; } = new();
}

/// <summary>
/// Adds redirect routes for the incoming route.
/// </summary>
public class RouteRedirect : RouteBase
{
    /// <summary>
    /// The redirect of the incoming route.
    /// </summary>
    [JsonProperty("redirect")]
    public string Redirect { get; set; } = default!;

    /// <summary>
    /// The status code, either 301 or 302.
    /// </summary>
    [JsonProperty("statusCode")]
    public int? StatusCode { get; set; } = default!;
}

