namespace Blog.Builder.Models.Crawlers;

/// <summary>
/// An object describing a calendar event.
/// </summary>
public class CalendarEvent
{
    /// <summary>
    /// The organizer of the event (e.g. Munich .NET Meetup)
    /// </summary>
    public string Organizer { get; set; } = default!;
    /// <summary>
    /// The URL of the organizers website.
    /// </summary>
    public string OrganizerUrl { get; set; } = default!;
    /// <summary>
    /// The title of the event.
    /// </summary>
    public string Title { get; set; } = default!;
    /// <summary>
    /// The date and time of the event.
    /// </summary>
    public DateTime DateTime { get; set; }
    /// <summary>
    /// The place of the event.
    /// </summary>
    public string Place { get; set; } = default!;
    /// <summary>
    /// The URL of the event.
    /// </summary>
    public string Url { get; set; } = default!;
}
