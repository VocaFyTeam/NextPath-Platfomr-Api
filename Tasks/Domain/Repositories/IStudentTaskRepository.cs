using NextPath.Platform.Shared.Domain.Repositories;
using NextPath.Platform.Tasks.Domain.Model.Aggregates;
using NextPath.Platform.Tasks.Domain.Model.ValueObjects;

namespace NextPath.Platform.Tasks.Domain.Repositories;

public interface IStudentTaskRepository : IBaseRepository<StudentTask>
{
    /// <summary>Tracked entity, for commands that modify it.</summary>
    Task<StudentTask?> FindByStudentTaskIdAsync(StudentTaskId studentTaskId,
        CancellationToken cancellationToken = default);

    Task<StudentTask?> FindByStudentTaskIdAsNoTrackingAsync(StudentTaskId studentTaskId,
        CancellationToken cancellationToken = default);

    /// <summary>Tasks of the student ordered by <c>Order</c> (then by id).</summary>
    Task<IEnumerable<StudentTask>> FindByStudentIdAsync(StudentId studentId,
        CancellationToken cancellationToken = default);

    Task<int> GetNextOrderByStudentIdAsync(StudentId studentId, CancellationToken cancellationToken = default);
}
