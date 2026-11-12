using VocaFy.NextPath.Platform.Monitoring.Domain.Model.ValueObjects;

namespace VocaFy.NextPath.Platform.Monitoring.Domain.Model.Commands;

public record RegisterStudentActivityCommand(StudentId StudentId, DateTimeOffset? ActivityDate)
{
    public RegisterStudentActivityCommand(Guid studentId, DateTimeOffset? activityDate)
        : this(new StudentId(studentId), activityDate)
    {
    }
}
