using VocaFy.NextPath.Platform.Monitoring.Domain.Model.ValueObjects;

namespace VocaFy.NextPath.Platform.Monitoring.Domain.Model.Queries;

public record GetAllStudentGroupsQuery(PsychologistId PsychologistId)
{
    public GetAllStudentGroupsQuery(Guid psychologistId) : this(new PsychologistId(psychologistId))
    {
    }
}
