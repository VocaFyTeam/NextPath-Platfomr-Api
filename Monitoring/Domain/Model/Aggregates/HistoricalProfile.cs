using NextPath.Platform.Monitoring.Domain.Model.Commands;
using NextPath.Platform.Monitoring.Domain.Model.Entities;
using NextPath.Platform.Monitoring.Domain.Model.Events;
using NextPath.Platform.Monitoring.Domain.Model.ValueObjects;
using NextPath.Platform.Monitoring.Domain.Services;
using NextPath.Platform.Shared.Domain.Model.Aggregates.Internal;

namespace NextPath.Platform.Monitoring.Domain.Model.Aggregates;


public partial class HistoricalProfile : AggregateRoot
{
    public const int MaxObservationSummaryLength = 240;


    public HistoricalProfile()
    {
        ObservationSummary = string.Empty;
        Flags = new List<EStudentFlag>();
        Observations = new List<ProfessionalObservation>();
        LastActivityAt = DateTimeOffset.UtcNow;
    }

    public HistoricalProfile(StudentId studentId, PsychologistId psychologistId, StudentGroupId? groupId,
        string? observationSummary) : this()
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(studentId.Value);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(psychologistId.Value);
        StudentId = studentId;
        AssignedPsychologistId = psychologistId;
        GroupId = groupId;
        ObservationSummary = observationSummary?.Trim() ?? string.Empty;
        AddDomainEvent(new HistoricalProfileCreatedEvent(studentId, psychologistId));
    }


    public HistoricalProfile(CreateHistoricalProfileCommand command)
        : this(command.StudentId, command.PsychologistId, command.GroupId, command.Observation)
    {
    }

    public HistoricalProfileId Id { get; }
    
    public StudentId StudentId { get; private set; }
    
    public PsychologistId AssignedPsychologistId { get; private set; }
    
    public StudentGroupId? GroupId { get; private set; }
    
    public string ObservationSummary { get; private set; }
    public List<EStudentFlag> Flags { get; private set; }
    
    public DateTimeOffset LastActivityAt { get; private set; }
    
    public ICollection<ProfessionalObservation> Observations { get; }
    
    public bool IsAssignedTo(PsychologistId psychologistId)
    {
        return AssignedPsychologistId == psychologistId;
    }
    
    public void UpdateObservationSummary(string observationSummary)
    {
        ObservationSummary = Summarize(observationSummary);
    }
    
    public void SetFlags(IEnumerable<EStudentFlag> flags)
    {
        Flags = flags.Distinct().ToList();
    }
    
    public void AssignToGroup(StudentGroupId groupId)
    {
        GroupId = groupId;
    }
    
    public ProfessionalObservation RecordObservation(PsychologistId psychologistId, string observationText,
        string? agreements, DateTimeOffset? sessionDate)
    {
        var observation = new ProfessionalObservation(Id, StudentId, psychologistId, observationText, agreements,
            sessionDate);
        Observations.Add(observation);
        ObservationSummary = Summarize(observation.ObservationText);
        AddDomainEvent(new ProfessionalObservationRecordedEvent(Id, StudentId, psychologistId));
        return observation;
    }
    
    public void RegisterActivity(DateTimeOffset activityAt)
    {
        if (activityAt > LastActivityAt) LastActivityAt = activityAt;
        Flags.Remove(EStudentFlag.Inactivity);
    }
    public int DaysInactive(DateTimeOffset now)
    {
        return InactivityPolicy.DaysInactive(LastActivityAt, now);
    }
    
    public void RefreshInactivityFlag(DateTimeOffset now)
    {
        var isInactive = InactivityPolicy.IsProlonged(LastActivityAt, now);
        if (isInactive && !Flags.Contains(EStudentFlag.Inactivity))
            Flags.Add(EStudentFlag.Inactivity);
        else if (!isInactive)
            Flags.Remove(EStudentFlag.Inactivity);
    }
    public IReadOnlyList<InterventionRecommendation> GetInterventionRecommendations()
    {
        return InterventionRecommendationPolicy.Recommend(Flags);
    }

    private static string Summarize(string text)
    {
        var trimmed = text?.Trim() ?? string.Empty;
        return trimmed.Length <= MaxObservationSummaryLength
            ? trimmed
            : string.Concat(trimmed.AsSpan(0, MaxObservationSummaryLength - 1), "\u2026");
    }
}
