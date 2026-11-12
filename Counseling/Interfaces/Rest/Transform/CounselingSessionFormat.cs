using System.Globalization;
using VocaFy.NextPath.Platform.Counseling.Domain.Model.ValueObjects;

namespace VocaFy.NextPath.Platform.Counseling.Interfaces.Rest.Transform;

/// <summary>
///     Translates between the domain values and the text the web app uses
///     (INDIVIDUAL / GROUP, SCHEDULED / COMPLETED / CANCELLED, dd/MM/yyyy and h:mm AM/PM).
/// </summary>
public static class CounselingSessionFormat
{
    private static readonly string[] DateFormats = ["dd/MM/yyyy", "d/M/yyyy", "yyyy-MM-dd"];
    private static readonly string[] TimeFormats = ["h:mm tt", "hh:mm tt", "h:mmtt", "H:mm", "HH:mm", "HH:mm:ss"];

    public static string ToApiType(ESessionType type) => type == ESessionType.Group ? "GROUP" : "INDIVIDUAL";

    public static bool TryParseType(string? value, out ESessionType type)
    {
        switch (value?.Trim().ToUpperInvariant())
        {
            case "INDIVIDUAL":
                type = ESessionType.Individual;
                return true;
            case "GROUP":
                type = ESessionType.Group;
                return true;
            default:
                type = default;
                return false;
        }
    }

    public static string ToApiStatus(ESessionStatus status) => status switch
    {
        ESessionStatus.Scheduled => "SCHEDULED",
        ESessionStatus.Completed => "COMPLETED",
        ESessionStatus.Cancelled => "CANCELLED",
        ESessionStatus.ConflictDetected => "CONFLICT_DETECTED",
        _ => status.ToString().ToUpperInvariant()
    };

    public static bool TryParseStatus(string? value, out ESessionStatus status)
    {
        switch (value?.Trim().Replace("_", string.Empty).ToUpperInvariant())
        {
            case "SCHEDULED":
                status = ESessionStatus.Scheduled;
                return true;
            case "COMPLETED":
                status = ESessionStatus.Completed;
                return true;
            case "CANCELLED":
            case "CANCELED":
                status = ESessionStatus.Cancelled;
                return true;
            case "CONFLICTDETECTED":
                status = ESessionStatus.ConflictDetected;
                return true;
            default:
                status = default;
                return false;
        }
    }

    public static bool TryParseSchedule(string? date, string? time, out DateTime scheduledDateTime)
    {
        scheduledDateTime = default;
        if (string.IsNullOrWhiteSpace(date) || string.IsNullOrWhiteSpace(time)) return false;

        if (!DateOnly.TryParseExact(date.Trim(), DateFormats, CultureInfo.InvariantCulture, DateTimeStyles.None,
                out var day))
            return false;
        if (!TimeOnly.TryParseExact(time.Trim().ToUpperInvariant(), TimeFormats, CultureInfo.InvariantCulture,
                DateTimeStyles.None, out var hour))
            return false;

        scheduledDateTime = day.ToDateTime(hour);
        return true;
    }

    public static string FormatDate(DateTime value) => value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

    public static string FormatTime(DateTime value) => value.ToString("h:mm tt", CultureInfo.InvariantCulture);

    public static string FormatIso(DateTime value) => value.ToString("yyyy-MM-dd'T'HH:mm:ss", CultureInfo.InvariantCulture);
}
