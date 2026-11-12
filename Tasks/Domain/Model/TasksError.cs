namespace NextPath.Platform.Tasks.Domain.Model;

public enum TasksError
{
    None,
    StudentTaskNotFound,
    InvalidTaskStatus,
    InvalidStudentId,
    OperationCancelled,
    DatabaseError,
    InternalServerError
}
