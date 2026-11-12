using VocaFy.NextPath.Platform.Monitoring.Domain.Model.ValueObjects;

namespace VocaFy.NextPath.Platform.Monitoring.Domain.Model.Queries;

public record GetStudentGroupByIdQuery(StudentGroupId StudentGroupId)
{
    public GetStudentGroupByIdQuery(int studentGroupId) : this(new StudentGroupId(studentGroupId))
    {
    }
}
