using VocaFy.NextPath.Platform.Counseling.Domain.Model.Events;
using VocaFy.NextPath.Platform.Counseling.Domain.Model.ValueObjects;

namespace VocaFy.NextPath.Platform.Counseling.Domain.Model.Aggregates;

/// <summary>
///     One-to-one session between a psychologist and a student (US05).
/// </summary>
public class IndividualSession : Session
{
    public IndividualSession()
    {
    }

    public IndividualSession(PsychologistId hostId, StudentId attendeeId, string sessionTitle,
        DateTime sessionDateTime, int sessionDuration, string? sessionNotes, string? sessionMeetLink)
        : base(hostId, sessionTitle, sessionDateTime, sessionDuration, sessionNotes, sessionMeetLink)
    {
        StudentId = attendeeId;
        AddDomainEvent(new IndividualSessionScheduledEvent(Id, PsychologistId, StudentId, ScheduledDateTime));
    }

    public StudentId StudentId { get; private set; }

    public override ESessionType Type => ESessionType.Individual;

    public override IReadOnlyCollection<StudentId> StudentIds => new[] { StudentId };

    public IndividualSession ReassignStudent(StudentId studentId)
    {
        EnsureScheduled();
        StudentId = studentId;
        return this;
    }
}
