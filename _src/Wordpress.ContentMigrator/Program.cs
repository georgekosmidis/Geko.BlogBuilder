using System.Text.RegularExpressions;
using Geko.HttpClientService;
using Geko.HttpClientService.Extensions;
using Newtonsoft.Json;

Console.WriteLine("Retrieving Posts");

var httpClientService = HttpClientServiceFactory.Instance.CreateHttpClientService();
var blogPostsResponse = await httpClientService.GetAsync<Feed>("https://blog.georgekosmidis.net/feed/json");
var ROOT = "..\\..\\..\\..\\..\\..\\raw\\";


if (blogPostsResponse is null)
{
    throw new NullReferenceException(nameof(blogPostsResponse));
}
if (blogPostsResponse.BodyAsType is null)
{
    throw new NullReferenceException(nameof(blogPostsResponse.BodyAsType));
}
if (blogPostsResponse.BodyAsType.Items is null)
{
    throw new NullReferenceException(nameof(blogPostsResponse.BodyAsType.Items));
}

var folderNum = 100000;
foreach (var item in blogPostsResponse.BodyAsType.Items.OrderBy(x => x.DatePublished))
{
    if (item.Id is null)
    {
        throw new NullReferenceException(nameof(item.Id));
    }
    if (item.ContentHtml is null)
    {
        throw new NullReferenceException(nameof(item.ContentHtml));
    }
    if (item.Url is null)
    {
        throw new NullReferenceException(nameof(item.Url));
    }
    Console.WriteLine($"Working with '{item.Title}'");

    //WP corrections
    item.ContentHtml = item.ContentHtml.Replace("\\n", "<br \\>");
    item.ContentHtml = Regex.Replace(item.ContentHtml,
                                    "https:\\/\\/blog\\.georgekosmidis\\.net\\/2[0-9]{3}\\/[01][0-9]\\/[0123][0-9]\\/([^\\s\"]+)",
                                    delegate (Match match)
                                    {
                                        if (match.Success && match.Groups.Count != 2)
                                        {
                                            throw new Exception("Match found but slang not captured!");
                                        }

                                        var relativeLink = "/" + match.Groups[1].Value.Trim('/') + "/";

                                        return relativeLink;
                                    });
    item.ContentHtml = Regex.Replace(item.ContentHtml, "<p>The post <a href=(.+) first appeared on(.+)Kosmidis</a>.</p>", string.Empty);
    item.ContentHtml = Regex.Replace(item.ContentHtml, "<img src=\"https://s.w.org/images/core/emoji/13.1.0/72x72/1f642.png\" alt=\"🙂\"[^>]*>", string.Empty);

    var key = item.Id.Split('=')[1];
    var slang = item.Url.Trim('/').Split('/').Last();
    var relativeUrl = "/" + slang + ".html";
    var title = item.Title ?? throw new NullReferenceException(nameof(item.Title));
    var relativeImageUrl = "";
    var datePublished = item.DatePublished ?? throw new NullReferenceException(nameof(item.DatePublished));
    var dateModified = item.DateModified ?? throw new NullReferenceException(nameof(item.DateModified));
    var tags = item.Tags ?? new List<string>();

    var contentArray = item.ContentHtml.Split($"<span id=\"more-{key}\"></span>");
    var description = contentArray[0];
    var body = contentArray[0];
    if (contentArray.Length == 2)
    {
        body += contentArray[1];
    }

    //Start saving
    folderNum += 10;
    var postDirectory = Path.Combine(ROOT, "posts", $"{folderNum}-{slang}");//datePublished:yyyyMMddHHmmss
    if (Directory.Exists(postDirectory))
    {
        Directory.Delete(postDirectory, true);
    }
    Directory.CreateDirectory(postDirectory);
    Directory.CreateDirectory(Path.Combine(postDirectory, "media"));

    //save feature image
    if (!string.IsNullOrWhiteSpace(item.Image))
    {
        var featureImageResponse = await httpClientService.GetAsync<Stream>(item.Image);// ?? throw new ArgumentNullException("Feature Image Response returned a null object");

        if (featureImageResponse.BodyAsStream is null)
        {
            throw new NullReferenceException(nameof(featureImageResponse.BodyAsStream));
        }

        await File.WriteAllBytesAsync(Path.Combine(postDirectory, "media", $"{folderNum}-feature.png"), Helpers.ReadFully(featureImageResponse.BodyAsStream));
        relativeImageUrl = $"/media/{folderNum}-feature.png";
    }

    //handle body images
    foreach (Match match in Regex.Matches(item.ContentHtml, "img[^>]src=\"([^\"\\?]+)\""))
    {
        if (match.Success && match.Groups.Count != 2)
        {
            throw new Exception("Match found with unexpected number of groups!");
        }

        var bodyImage = await httpClientService.GetAsync<Stream>(match.Groups[1].Value);
        if (bodyImage.BodyAsStream is null)
        {
            throw new NullReferenceException(nameof(bodyImage.BodyAsStream));
        }

        var imageName = $"{folderNum}-{new Uri(match.Groups[1].Value).Segments.Last()}";
        body = body.Replace(match.Groups[1].Value, "media/" + imageName, StringComparison.InvariantCultureIgnoreCase);
        await File.WriteAllBytesAsync(Path.Combine(postDirectory, "media", imageName), Helpers.ReadFully(bodyImage.BodyAsStream));
    }

    //handle gists
    foreach (Match match in Regex.Matches(item.ContentHtml, "https:\\/\\/gist\\.github\\.com\\/georgekosmidis\\/([^.]+)\\.js\\?file=([^\"]+)"))
    {
        if (match.Success && match.Groups.Count != 3)
        {
            throw new Exception("Match found with unexpected number of groups!");
        }

        var jsScript = await httpClientService.GetAsync(match.Value.Trim());
        if (jsScript.HasError || jsScript.BodyAsString is null)
        {
            throw new NullReferenceException(nameof(jsScript.BodyAsString));
        }

        var rawCodeMatch = Regex.Match(jsScript.BodyAsString, $"https:\\/\\/gist\\.github\\.com\\/georgekosmidis\\/{match.Groups[1].Value}\\/raw\\/([^\"]+)\\/{match.Groups[2].Value}");
        var rawCodeResponse = await httpClientService.GetAsync(rawCodeMatch.Value);
        if (rawCodeResponse.HasError || rawCodeResponse.BodyAsString is null)
        {
            throw new NullReferenceException(nameof(rawCodeResponse.BodyAsString));
        }
        var rawCode = rawCodeResponse.BodyAsString.Replace("<", "&lt;").Replace(">", "&gt;");

        body = body.Replace($"<script src=\"{match.Value}\"></script>", $"<pre><code class=\"language-csharp\">{rawCode}</code></pre>", StringComparison.InvariantCultureIgnoreCase);
        //await File.WriteAllBytesAsync(Path.Combine(postDirectory, "media", imageName), Helpers.ReadFully(bodyImage.BodyAsStream));
    }

    var migratedPost = new MigratedPost(relativeUrl, title, description, datePublished, dateModified, relativeImageUrl, tags);

    //save json
    var json = JsonConvert.SerializeObject(migratedPost, Formatting.Indented);
    File.WriteAllText(Path.Combine(postDirectory, "content.json"), json);

    //save body
    File.WriteAllText(Path.Combine(postDirectory, "content.html"), body);

}

// string jsonString = JsonSerializer.(migratedPost);
