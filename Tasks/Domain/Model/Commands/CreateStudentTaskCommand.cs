using NextPath.Platform.Tasks.Domain.Model.ValueObjects;

namespace NextPath.Platform.Tasks.Domain.Model.Commands;

/// <summary><c>Order</c> is optional: when missing, the task goes to the end of the student's plan.</summary>
public record CreateStudentTaskCommand(StudentId StudentId, string Title, DateOnly? DueDate, int? Order);
