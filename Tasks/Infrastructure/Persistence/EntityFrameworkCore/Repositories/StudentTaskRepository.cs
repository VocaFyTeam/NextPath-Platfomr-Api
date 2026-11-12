using Microsoft.EntityFrameworkCore;
using VocaFy.NextPath.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using VocaFy.NextPath.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using VocaFy.NextPath.Platform.Tasks.Domain.Model.Aggregates;
using VocaFy.NextPath.Platform.Tasks.Domain.Model.ValueObjects;
using VocaFy.NextPath.Platform.Tasks.Domain.Repositories;

namespace VocaFy.NextPath.Platform.Tasks.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class StudentTaskRepository(AppDbContext context)
    : BaseRepository<StudentTask>(context), IStudentTaskRepository
{
    public async Task<StudentTask?> FindByStudentTaskIdAsync(StudentTaskId studentTaskId,
        CancellationToken cancellationToken = default)
    {
        return await Context.Set<StudentTask>()
            .FirstOrDefaultAsync(task => task.Id == studentTaskId, cancellationToken);
    }

    public async Task<StudentTask?> FindByStudentTaskIdAsNoTrackingAsync(StudentTaskId studentTaskId,
        CancellationToken cancellationToken = default)
    {
        return await Context.Set<StudentTask>()
            .AsNoTracking()
            .FirstOrDefaultAsync(task => task.Id == studentTaskId, cancellationToken);
    }

    public async Task<IEnumerable<StudentTask>> FindByStudentIdAsync(StudentId studentId,
        CancellationToken cancellationToken = default)
    {
        return await Context.Set<StudentTask>()
            .AsNoTracking()
            .Where(task => task.StudentId == studentId)
            .OrderBy(task => task.Order)
            .ThenBy(task => task.Id)
            .ToListAsync(cancellationToken);
    }

    public async Task<int> GetNextOrderByStudentIdAsync(StudentId studentId,
        CancellationToken cancellationToken = default)
    {
        var highest = await Context.Set<StudentTask>()
            .Where(task => task.StudentId == studentId)
            .MaxAsync(task => (int?)task.Order, cancellationToken);
        return (highest ?? 0) + 1;
    }
}
