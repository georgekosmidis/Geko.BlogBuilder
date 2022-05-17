using Blog.Builder.Models.Crawlers;

namespace Blog.Builder.Interfaces.Crawlers;

/// <summary>
/// Crawls a folder for json files that follow the <see cref="CalendarEvent"/>.
/// </summary>
internal interface IFileEventCrawler
{
    /// <summary>
    /// Scans the <paramref name="directory"/> for json files representing a <see cref="CalendarEvent"/>.
    /// </summary>
    /// <param name="directory">The directory where the folders with the json files live.</param>
    /// <returns>A list of <see cref="CalendarEvent"/> build from the corresponding json files in <paramref name="directory"/>.</returns>
    IList<CalendarEvent> Get(string directory);
}