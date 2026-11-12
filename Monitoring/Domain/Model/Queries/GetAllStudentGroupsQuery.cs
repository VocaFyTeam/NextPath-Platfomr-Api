using NextPath.Platform.Monitoring.Domain.Model.ValueObjects;

namespace NextPath.Platform.Monitoring.Domain.Model.Queries;

public record GetAllStudentGroupsQuery(PsychologistId PsychologistId)
{
    public GetAllStudentGroupsQuery(int psychologistId) : this(new PsychologistId(psychologistId))
    {
    }
}
