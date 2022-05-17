using Blog.Builder.Models.Crawlers;

namespace Blog.Builder.Interfaces.Crawlers;

/// <summary>
/// Crawls meetup.com iCalendars
/// </summary>
internal interface IMeetupEventCrawler
{
    /// <summary>
    /// Crawls the <paramref name="iCalendarUrl"/> for events and adds them as <see cref="CalendarEvent"/>.
    /// </summary>
    /// <param name="ogranizer">The name of the community (e.g. Munich .NET Meetup).</param>
    /// <param name="organizerUrl">The URL of the community.</param>
    /// <param name="iCalendarUrl">The URL of the iCalendar.</param>
    /// <returns></returns>
    Task<IList<CalendarEvent>> GetAsync(string ogranizer, Uri organizerUrl, Uri iCalendarUrl);
}