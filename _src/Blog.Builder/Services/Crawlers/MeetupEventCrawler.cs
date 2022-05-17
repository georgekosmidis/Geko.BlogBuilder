using Blog.Builder.Exceptions;
using Blog.Builder.Interfaces.Crawlers;
using Blog.Builder.Models.Crawlers;
using Geko.HttpClientService;
using Geko.HttpClientService.Extensions;
using Ical.Net;

namespace Blog.Builder.Services.Crawlers;

/// <inheritdoc/>
internal class MeetupEventCrawler : IMeetupEventCrawler
{
    /// <summary>
    /// The Geko.HttpClientService.
    /// </summary>
    private readonly HttpClientService _httpClientService;

    public MeetupEventCrawler(IHttpClientServiceFactory requestServiceFactory)
    {
        _httpClientService = requestServiceFactory.CreateHttpClientService();
    }

    /// <inheritdoc/>
    public async Task<IList<CalendarEvent>> GetAsync(string ogranizer, Uri organizerUrl, Uri iCalendarUrl)
    {
        ExceptionHelpers.ThrowIfNullOrWhiteSpace(ogranizer);
        ArgumentNullException.ThrowIfNull(organizerUrl);
        ArgumentNullException.ThrowIfNull(iCalendarUrl);

        var iCalResult = await _httpClientService.GetAsync(iCalendarUrl.ToString());
        if (iCalResult.HasError)
        {
            throw new Exception($"An error occured while loading the calendar: {iCalResult.Error}.");
        }
        ExceptionHelpers.ThrowIfNullOrWhiteSpace(iCalResult.BodyAsString);

        //throws an exception if the format is wrong
        var calendar = Calendar.Load(iCalResult.BodyAsString);

        var eventList = new List<CalendarEvent>();

        foreach (var evnt in calendar.Events)
        {
            eventList.Add(new CalendarEvent
            {
                Organizer = ogranizer,
                OrganizerUrl = organizerUrl.ToString(),
                Title = evnt.Summary,
                DateTime = evnt.Start.AsUtc,
                Place = evnt.Location,
                Url = evnt.Url.ToString(),

            });
        }

        return eventList;
    }
}
