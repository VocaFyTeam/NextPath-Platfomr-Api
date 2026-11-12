using VocaFy.NextPath.Platform.Monitoring.Domain.Model.ValueObjects;

namespace VocaFy.NextPath.Platform.Monitoring.Domain.Model.Queries;

public record GetAllGroupComparisonReportsQuery(PsychologistId PsychologistId)
{
    public GetAllGroupComparisonReportsQuery(int psychologistId) : this(new PsychologistId(psychologistId))
    {
    }
}
