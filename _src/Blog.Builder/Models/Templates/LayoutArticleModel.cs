using Blog.Builder.Exceptions;
using System.Text;

namespace Blog.Builder.Models.Templates;

/// <summary>
/// Used for an article page (template-article.cshtml).
/// Inherits all members of <see cref="LayoutModelBase"/>.
/// </summary>
public record class LayoutArticleModel : LayoutModelBase
{

    /// <summary>
    /// Ctor.
    /// </summary>
    /// <param name="appSettings">The appsettings.json model</param>
    public LayoutArticleModel(AppSettings appSettings) : base(appSettings)
    {
        TemplateDataModel = nameof(LayoutArticleModel);
    }

    /// <summary>
    /// The calculated path for the small version of the image.
    /// The smaller version is automatically created from <see cref="Services.PageProcessor.ProcessPage{T}(string)"/>.
    /// </summary>
    public string? RelativeImageUrlSmall => RelativeImageUrl is null
                ? null
                : (Path.GetDirectoryName(RelativeImageUrl) ?? string.Empty).Replace("\\", "/")
                    + "/"
                    + Path.GetFileNameWithoutExtension(RelativeImageUrl) + "-small" + Path.GetExtension(RelativeImageUrl);

    /// <summary>
    /// A text representation of a TimeSpan for the modification date.
    /// See also <see cref="SpanCalculation"/> and <see cref="LayoutModelBase.DateModified"/>.
    /// </summary>
    public string DateModifiedText
    {
        get
        {
            var span = DateTime.Now - DateModified;

            return SpanCalculation(span);
        }
    }

    /// <summary>
    /// A text representation of a TimeSpan for the published date.
    /// See also <see cref="SpanCalculation"/> and <see cref="LayoutModelBase.DatePublished"/>.
    /// </summary>
    public string DatePublishedText
    {
        get
        {
            var span = DateTime.Now - DatePublished;

            return SpanCalculation(span);
        }
    }

    /// <summary>
    /// Calculates a user friendly representation of a <seealso cref="TimeSpan"/> in the formath " x minutes/hours/weeks... ago"
    /// </summary>
    /// <remarks>todo: The code suffers from too many ifs, a clearer way must be found.</remarks>
    /// <param name="span">The <seealso cref="TimeSpan"/> to be described.</param>
    /// <returns>Returns the calculated string</returns>
    private static string SpanCalculation(TimeSpan span)
    {
        StringBuilder? result = new();

        var years = Math.Round(span.Days / 365d);
        var months = Math.Max(0, Math.Round((span.Days - (years * 365)) / 30));
        var weeks = Math.Max(0, Math.Round((span.Days - (years * 365) - (months * 30)) / 7));
        var days = Math.Max(0, span.Days - (weeks * 7) - (months * 30) - (years * 365));

        if (years > 0)
        {
            result.Append(years);
            result.Append(years == 1 ? " year" : " years");
        }
        if (months > 0)
        {
            if (years > 0)
            {
                result.Append(" and ");
            }
            result.Append(months);
            result.Append(months == 1 ? " month" : " months");
        }
        //don't show days for more than a year old articles
        if (years == 0)
        {
            if (weeks > 0)
            {
                if (months > 0)
                {
                    result.Append(" and ");
                }
                result.Append(weeks);
                result.Append(weeks == 1 ? " week" : " weeks");
            }
            //don't show days if we have weeks or months
            if (months == 0 && weeks == 0 && days > 0)
            {
                if (months > 0)
                {
                    result.Append(" and ");
                }
                result.Append(days);
                result.Append(days == 1 ? " day" : " days");
            }
        }

        //if no years, months or days, then check smaller time parts
        if (result.Length == 0)
        {
            if (span.Hours > 0)
            {
                result.Append(span.Hours);
                result.Append(span.Hours == 1 ? " hour" : " hours");
            }
            if (span.Hours == 0 && span.Minutes > 0)
            {
                result.Append(span.Minutes);
                result.Append(span.Minutes == 1 ? " minute" : " minutes");
            }
        }

        return result.Length == 0 ? "a few seconds ago" : result.Append(" ago").ToString();
    }

    /// <summary>
    /// Validates what this object knows and throws an exception if something is wrong.
    /// Check the <see cref="Validate"/> source code for the validations.
    /// </summary>
    public new void Validate()
    {
        base.Validate();

        ExceptionHelpers.ThrowIfNullOrWhiteSpace(RelativeImageUrlSmall);
        ExceptionHelpers.ThrowIfNullOrWhiteSpace(DateModifiedText);
        ExceptionHelpers.ThrowIfNullOrWhiteSpace(DatePublishedText);
        ExceptionHelpers.ThrowIfNullOrWhiteSpace(Body);

        if (TemplateDataModel != nameof(LayoutArticleModel))
        {
            throw new Exception($"{nameof(TemplateDataModel)} must be {nameof(LayoutArticleModel)} for the type {nameof(LayoutArticleModel)}.");
        }
    }
}
