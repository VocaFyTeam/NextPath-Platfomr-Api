using VocaFy.NextPath.Platform.Shared.Domain.Model.Errors;

namespace VocaFy.NextPath.Platform.Tasks.Domain.Model.Errors;

public static class TasksErrors
{
    public static readonly Error StudentTaskNotFound =
        Error.NotFound("Tasks.StudentTaskNotFound", "The specified task was not found.");

    public static readonly Error InvalidTaskStatus =
        Error.Validation("Tasks.InvalidTaskStatus", "The status must be 'pending' or 'completed'.");

    public static readonly Error InvalidStudentId =
        Error.Validation("Tasks.InvalidStudentId", "A valid studentId is required.");

    public static readonly Error DatabaseError =
        Error.Failure("Tasks.DatabaseError", "A database error occurred.");
}
