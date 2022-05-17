namespace Blog.Builder.Models;

/// <summary>
/// The AppSettings of this app.
/// </summary>
public record class AppSettings
{
    /// <summary>
    /// The number of cards per page (defauls is 9).
    /// </summary>
    public int CardsPerPage { get; init; } = 9;

    /// <summary>
    /// The blog title, displayed in index pages.
    /// </summary>
    public string BlogTitle { get; init; } = default!;

    /// <summary>
    /// The blog description, displayed in index pages.
    /// </summary>
    public string BlogDescription { get; init; } = default!;

    /// <summary>
    /// The blog tags, displayed in index pages.
    /// </summary>
    public List<string> BlogTags { get; init; } = new List<string>();

    private string blogUrl = default!;
    /// <summary>
    /// The blog base URL.
    /// </summary>
    public string BlogUrl
    {
        get
        {
            //ugly hack because I always forget to change the appsettings blogUrl
            if (blogUrl is not default(string?) and not "https://blog.georgekosmidis.net")
            {
#if RELEASE
                throw new Exception("You forgot to change the BlogUrl from the appsettings, AGAIN!");
#endif
            }


            return blogUrl!;
        }
        init => blogUrl = value;
    }

    /// <summary>
    /// The blog image
    /// </summary>
    public string BlogImage { get; init; } = default!;

    /// <summary>
    /// The twitter handle of the author (or blog)
    /// </summary>
    public string TwitterHandle { get; init; } = default!;

    /// <summary>
    /// The author's personal page
    /// </summary>
    public string AuthorPersonalPage { get; init; } = default!;

    /// <summary>
    /// The author name of the blog posts
    /// </summary>
    public string AuthorName { get; init; } = default!;

    /// <summary>
    /// The name of the meetup.com usergroup that its events will be crawled.
    /// </summary>
    public string MeetupUserGroupName { get; init; } = default!;

    /// <summary>
    /// The URL of the meetup.com usergroup.
    /// </summary>
    public string MeetupUserGroupUrl { get; init; } = default!;

    /// <summary>
    /// The URL of the ICal of the meetup.com usergroup.
    /// </summary>
    public string MeetupUserGroupIcalUrl { get; init; } = default!;

    /// <summary>
    /// The URL of the workables folder in the Github repo.
    /// </summary>
    public string GithubRepoUrl { get; init; } = default!;

    /// <summary>
    /// Microsoft CreatorID for skilling
    /// </summary>
    public string MicrosoftCreatorID { get; init; } = default!;
}

