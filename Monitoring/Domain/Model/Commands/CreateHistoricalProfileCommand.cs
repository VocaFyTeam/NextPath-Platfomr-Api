using NextPath.Platform.Monitoring.Domain.Model.ValueObjects;

namespace NextPath.Platform.Monitoring.Domain.Model.Commands;

public record CreateHistoricalProfileCommand(
    StudentId StudentId,
    PsychologistId PsychologistId,
    StudentGroupId? GroupId,
    string Observation)
{
    public CreateHistoricalProfileCommand(int studentId, int psychologistId, int? groupId, string observation)
        : this(new StudentId(studentId), new PsychologistId(psychologistId),
            groupId.HasValue ? new StudentGroupId(groupId.Value) : null, observation)
    {
    }
}
