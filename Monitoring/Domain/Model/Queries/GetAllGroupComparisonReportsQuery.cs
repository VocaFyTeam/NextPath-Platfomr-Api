using NextPath.Platform.Monitoring.Domain.Model.ValueObjects;

namespace NextPath.Platform.Monitoring.Domain.Model.Queries;

public record GetAllGroupComparisonReportsQuery(PsychologistId PsychologistId)
{
    public GetAllGroupComparisonReportsQuery(int psychologistId) : this(new PsychologistId(psychologistId))
    {
    }
}
