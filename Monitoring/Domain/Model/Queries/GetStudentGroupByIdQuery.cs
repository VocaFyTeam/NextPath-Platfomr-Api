using NextPath.Platform.Monitoring.Domain.Model.ValueObjects;

namespace NextPath.Platform.Monitoring.Domain.Model.Queries;

public record GetStudentGroupByIdQuery(StudentGroupId StudentGroupId)
{
    public GetStudentGroupByIdQuery(int studentGroupId) : this(new StudentGroupId(studentGroupId))
    {
    }
}
