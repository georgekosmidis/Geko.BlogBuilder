using Blog.Builder.Models.Crawlers;

namespace Blog.Builder.Models.Templates;

/// <summary>
/// A card template model that displays a list of events .
/// </summary>
public record class CardCalendarEventsModel : CardModelBase
{
    /// <summary>
    /// Ctor.
    /// </summary>
    /// <param name="appSettings">The appsettings.json model.</param>
    public CardCalendarEventsModel(AppSettings appSettings) : base(appSettings)
    {
        TemplateDataModel = nameof(CardCalendarEventsModel);
    }

    /// <summary>
    /// The calendar events (meetups or file events) to be parted in the template.
    /// </summary>
    public IList<CalendarEvent> CalendarEvents { get; set; } = new List<CalendarEvent>();

    /// <summary>
    /// Validates what this object knows and throws an exception if something is wrong.
    /// Check the <see cref="Validate"/> source code for the validations.
    /// </summary>
    public new void Validate()
    {
        base.Validate();
    }
}