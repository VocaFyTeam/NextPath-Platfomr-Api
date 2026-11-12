using NextPath.Platform.Tasks.Domain.Model.ValueObjects;

namespace NextPath.Platform.Tasks.Domain.Model.Commands;

public record ChangeStudentTaskStatusCommand(StudentTaskId StudentTaskId, ETaskStatus Status);
