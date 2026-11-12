using NextPath.Platform.Monitoring.Domain.Model.ValueObjects;

namespace NextPath.Platform.Monitoring.Domain.Model.Commands;

public record RegisterStudentActivityCommand(StudentId StudentId, DateTimeOffset? ActivityDate)
{
    public RegisterStudentActivityCommand(int studentId, DateTimeOffset? activityDate)
        : this(new StudentId(studentId), activityDate)
    {
    }
}
