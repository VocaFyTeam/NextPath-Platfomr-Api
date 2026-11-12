using VocaFy.NextPath.Platform.Counseling.Domain.Model.Events;
using VocaFy.NextPath.Platform.Counseling.Domain.Model.ValueObjects;
using VocaFy.NextPath.Platform.Shared.Domain.Model.Aggregates.Internal;

namespace VocaFy.NextPath.Platform.Counseling.Domain.Model.Aggregates;

/// <summary>
///     Orientation session that a psychologist schedules (abstract Session of the class diagram 4.7.1.8).
///     Specialized in <see cref="IndividualSession" /> and <see cref="GroupSession" />; both live in table
///     sessions, told apart by the session_type column.
/// </summary>
public abstract partial class Session : AggregateRoot
{
    public const int DefaultDurationMinutes = 60;
    public const int MinDurationMinutes = 15;
    public const int MaxDurationMinutes = 240;

    protected Session()
    {
        Title = string.Empty;
        Notes = string.Empty;
        MeetLink = string.Empty;
        Status = ESessionStatus.Scheduled;
    }

    // scheduleSession() of the diagram. Parameter names differ from property names so EF Core uses the
    // parameterless constructor on load.
    protected Session(PsychologistId hostId, string sessionTitle, DateTime sessionDateTime, int sessionDuration,
        string? sessionNotes, string? sessionMeetLink) : this()
    {
        Id = SessionId.New();
        PsychologistId = hostId;
        Status = ESessionStatus.Scheduled;
        UpdateDetails(sessionTitle, sessionDateTime, sessionDuration, sessionNotes, sessionMeetLink);
    }

    public SessionId Id { get; private set; }
    public PsychologistId PsychologistId { get; private set; }
    public string Title { get; private set; }
    public DateTime ScheduledDateTime { get; private set; }
    public int DurationMinutes { get; private set; }
    public ESessionStatus Status { get; private set; }
    public string Notes { get; private set; }
    public string MeetLink { get; private set; }

    /// <summary>INDIVIDUAL or GROUP. Not mapped: EF Core stores it as the discriminator.</summary>
    public abstract ESessionType Type { get; }

    /// <summary>Every student invited to the session. Not mapped.</summary>
    public abstract IReadOnlyCollection<StudentId> StudentIds { get; }

    public DateTime EndDateTime => ScheduledDateTime.AddMinutes(DurationMinutes);

    public bool IsScheduled => Status == ESessionStatus.Scheduled;

    public bool IsHostedBy(PsychologistId psychologistId) => PsychologistId == psychologistId;

    public bool Involves(StudentId studentId) => StudentIds.Contains(studentId);

    public bool IsInThePast(DateTime now) => ScheduledDateTime < now;

    /// <summary>True when [start, start + duration) intersects this session.</summary>
    public bool OverlapsWith(DateTime start, int durationMinutes) =>
        ScheduledDateTime < start.AddMinutes(durationMinutes) && start < EndDateTime;

    public static bool IsValidDuration(int durationMinutes) =>
        durationMinutes is >= MinDurationMinutes and <= MaxDurationMinutes;

    public Session UpdateDetails(string title, DateTime scheduledDateTime, int durationMinutes, string? notes,
        string? meetLink)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(title);
        if (!IsValidDuration(durationMinutes))
            throw new ArgumentOutOfRangeException(nameof(durationMinutes), durationMinutes,
                $"The duration must be between {MinDurationMinutes} and {MaxDurationMinutes} minutes.");
        EnsureScheduled();

        Title = title.Trim();
        ScheduledDateTime = scheduledDateTime;
        DurationMinutes = durationMinutes;
        Notes = notes?.Trim() ?? string.Empty;
        MeetLink = meetLink?.Trim() ?? string.Empty;
        return this;
    }

    /// <summary>saveNotes() of the diagram. Allowed in any status (e.g. notes after a completed session).</summary>
    public Session SaveNotes(string? notes)
    {
        Notes = notes?.Trim() ?? string.Empty;
        return this;
    }

    /// <summary>cancelSession() of the diagram.</summary>
    public Session CancelSession()
    {
        EnsureScheduled();
        Status = ESessionStatus.Cancelled;
        AddDomainEvent(new SessionCancelledEvent(Id, PsychologistId,
            StudentIds.Select(studentId => studentId.Value).ToList(), ScheduledDateTime));
        return this;
    }

    public Session CompleteSession()
    {
        EnsureScheduled();
        Status = ESessionStatus.Completed;
        return this;
    }

    protected void EnsureScheduled()
    {
        if (!IsScheduled)
            throw new InvalidOperationException("Only scheduled sessions can be changed.");
    }
}
