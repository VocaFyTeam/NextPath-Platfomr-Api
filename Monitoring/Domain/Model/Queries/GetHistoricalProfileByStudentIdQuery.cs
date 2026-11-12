using VocaFy.NextPath.Platform.Monitoring.Domain.Model.ValueObjects;

namespace VocaFy.NextPath.Platform.Monitoring.Domain.Model.Queries;

public record GetHistoricalProfileByStudentIdQuery(StudentId StudentId)
{
    public GetHistoricalProfileByStudentIdQuery(int studentId) : this(new StudentId(studentId))
    {
    }
}
