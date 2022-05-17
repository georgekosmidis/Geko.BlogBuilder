internal record class MigratedPost
{

    public MigratedPost(string relativeUrl, string title, string description, DateTime datePublished, DateTime dateModified, string? relativeImageUrl, List<string> tags)
    {
        RelativeUrl = relativeUrl;
        Title = title;
        Description = description;
        DatePublished = datePublished;
        DateModified = dateModified;
        RelativeImageUrl = relativeImageUrl;
        Tags = tags;
    }

    public string Type { get; init; } = "article";

    public string RelativeUrl { get; init; }

    public string Title { get; init; }

    public string Description { get; init; }

    public DateTime DatePublished { get; init; }

    public DateTime DateModified { get; init; }

    public string? RelativeImageUrl { get; init; }

    public List<string> Tags { get; init; }

}
