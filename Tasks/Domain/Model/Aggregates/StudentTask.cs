using VocaFy.NextPath.Platform.Shared.Domain.Model.Aggregates.Internal;
using VocaFy.NextPath.Platform.Shared.Domain.Model.Entities;
using VocaFy.NextPath.Platform.Tasks.Domain.Model.Commands;
using VocaFy.NextPath.Platform.Tasks.Domain.Model.Events;
using VocaFy.NextPath.Platform.Tasks.Domain.Model.ValueObjects;

namespace VocaFy.NextPath.Platform.Tasks.Domain.Model.Aggregates;

/// <summary>A task of the personal vocational plan of a student (HU22, HU23, HU24).</summary>
public class StudentTask : AggregateRoot, IAuditableEntity
{
    public const int MaxTitleLength = 200;

    // Required by EF Core.
    protected StudentTask()
    {
        Title = string.Empty;
    }

    public StudentTask(StudentId studentId, string title, DateOnly? dueDate, int order)
    {
        ValidateTitle(title);
        ArgumentOutOfRangeException.ThrowIfNegative(order);

        StudentId = studentId;
        Title = title.Trim();
        DueDate = dueDate;
        Order = order;
        Status = ETaskStatus.Pending;
    }

    /// <param name="command">Command with a resolved order (the command service computes it when missing).</param>
    public StudentTask(CreateStudentTaskCommand command)
        : this(command.StudentId, command.Title, command.DueDate, command.Order ?? 0)
    {
    }

    public StudentTaskId Id { get; private set; }
    public StudentId StudentId { get; private set; }
    public string Title { get; private set; }
    public ETaskStatus Status { get; private set; }
    public DateOnly? DueDate { get; private set; }
    public int Order { get; private set; }

    public bool IsCompleted => Status == ETaskStatus.Completed;

    public DateTimeOffset? CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }

    /// <summary>Marks the task as pending or completed. Completing it raises <see cref="StudentTaskCompletedEvent"/>.</summary>
    public StudentTask ChangeStatus(ETaskStatus status)
    {
        if (Status == status) return this;

        Status = status;
        if (status == ETaskStatus.Completed) AddDomainEvent(new StudentTaskCompletedEvent(StudentId, Id));
        return this;
    }

    public StudentTask Update(string title, DateOnly? dueDate, int order, ETaskStatus status)
    {
        ValidateTitle(title);
        ArgumentOutOfRangeException.ThrowIfNegative(order);

        Title = title.Trim();
        DueDate = dueDate;
        Order = order;
        return ChangeStatus(status);
    }

    private static void ValidateTitle(string title)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(title);
        if (title.Trim().Length > MaxTitleLength)
            throw new ArgumentException($"Title cannot be longer than {MaxTitleLength} characters.", nameof(title));
    }
}
