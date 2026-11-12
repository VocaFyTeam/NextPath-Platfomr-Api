using VocaFy.NextPath.Platform.Monitoring.Domain.Model.ValueObjects;

namespace VocaFy.NextPath.Platform.Monitoring.Domain.Model.Commands;

public record UpdateHistoricalProfileCommand(
    HistoricalProfileId HistoricalProfileId,
    PsychologistId PsychologistId,
    string? Observation,
    IReadOnlyCollection<EStudentFlag>? Flags,
    StudentGroupId? GroupId)
{
    public UpdateHistoricalProfileCommand(int historicalProfileId, int psychologistId, string? observation,
        IReadOnlyCollection<EStudentFlag>? flags, int? groupId)
        : this(new HistoricalProfileId(historicalProfileId), new PsychologistId(psychologistId), observation, flags,
            groupId.HasValue ? new StudentGroupId(groupId.Value) : null)
    {
    }
}
