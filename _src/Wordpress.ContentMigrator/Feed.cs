// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
using Newtonsoft.Json;

internal class Author
{
    [JsonProperty("name")]
    public string? Name { get; set; }

    [JsonProperty("url")]
    public string? Url { get; set; }

    [JsonProperty("avatar")]
    public string? Avatar { get; set; }
}

internal class Item
{
    [JsonProperty("id")]
    public string? Id { get; set; }

    [JsonProperty("url")]
    public string? Url { get; set; }

    [JsonProperty("title")]
    public string? Title { get; set; }

    [JsonProperty("content_html")]
    public string? ContentHtml { get; set; }

    [JsonProperty("content_text")]
    public string? ContentText { get; set; }

    [JsonProperty("date_published")]
    public DateTime? DatePublished { get; set; }

    [JsonProperty("date_modified")]
    public DateTime? DateModified { get; set; }

    [JsonProperty("authors")]
    public List<Author>? Authors { get; set; }

    [JsonProperty("author")]
    public Author? Author { get; set; }

    [JsonProperty("image")]
    public string? Image { get; set; }

    [JsonProperty("tags")]
    public List<string>? Tags { get; set; }
}

internal class Feed
{
    [JsonProperty("version")]
    public string? Version { get; set; }

    [JsonProperty("user_comment")]
    public string? UserComment { get; set; }

    [JsonProperty("home_page_url")]
    public string? HomePageUrl { get; set; }

    [JsonProperty("feed_url")]
    public string? FeedUrl { get; set; }

    [JsonProperty("language")]
    public string? Language { get; set; }

    [JsonProperty("title")]
    public string? Title { get; set; }

    [JsonProperty("description")]
    public string? Description { get; set; }

    [JsonProperty("icon")]
    public string? Icon { get; set; }

    [JsonProperty("items")]
    public List<Item>? Items { get; set; }
}

