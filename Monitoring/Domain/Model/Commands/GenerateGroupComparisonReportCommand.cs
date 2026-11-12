using VocaFy.NextPath.Platform.Monitoring.Domain.Model.ValueObjects;

namespace VocaFy.NextPath.Platform.Monitoring.Domain.Model.Commands;

public record GenerateGroupComparisonReportCommand(PsychologistId PsychologistId, IReadOnlyCollection<StudentGroupId> GroupIds)
{
    public GenerateGroupComparisonReportCommand(Guid psychologistId, IEnumerable<int> groupIds)
        : this(new PsychologistId(psychologistId), groupIds.Select(id => new StudentGroupId(id)).ToList())
    {
    }
}
