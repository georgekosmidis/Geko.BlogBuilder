using Blog.Builder.Exceptions;
using Blog.Builder.Interfaces.Crawlers;
using Blog.Builder.Models.Crawlers;
using Newtonsoft.Json;

namespace Blog.Builder.Services.Crawlers;

/// <inheritdoc/>
internal class FileEventCrawler : IFileEventCrawler
{

    public FileEventCrawler() { }

    /// <inheritdoc/>
    public IList<CalendarEvent> Get(string directory)
    {
        ExceptionHelpers.ThrowIfPathNotExists(directory);

        var eventList = new List<CalendarEvent>();

        //Read the card.json and valdiate the data found
        foreach (var dir in Directory.GetDirectories(directory))
        {
            var eventFile = Path.Combine(dir, Globals.EventJsonFilename);
            if (!File.Exists(eventFile))
            {
                continue;
            }
            var jsonFileContent = File.ReadAllText(eventFile);
            var calendarEventData = JsonConvert.DeserializeObject<CalendarEvent>(jsonFileContent);

            ExceptionHelpers.ThrowIfNull(calendarEventData);
            eventList.Add(calendarEventData);
        }

        return eventList;
    }
}
