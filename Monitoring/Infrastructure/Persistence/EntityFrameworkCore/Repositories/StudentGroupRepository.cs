using VocaFy.NextPath.Platform.Monitoring.Domain.Model.Aggregates;
using VocaFy.NextPath.Platform.Monitoring.Domain.Model.ValueObjects;
using VocaFy.NextPath.Platform.Monitoring.Domain.Repositories;
using VocaFy.NextPath.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using VocaFy.NextPath.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Microsoft.EntityFrameworkCore;

namespace VocaFy.NextPath.Platform.Monitoring.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class StudentGroupRepository(AppDbContext context) : BaseRepository<StudentGroup>(context), IStudentGroupRepository
{


    public async Task<StudentGroup?> FindByIdAsync(int id, CancellationToken cancellationToken)
    {
        var groupId = new StudentGroupId(id);
        return await Context.Set<StudentGroup>().FirstOrDefaultAsync(group => group.Id == groupId, cancellationToken);
    }


    public async Task<StudentGroup?> FindByIdAsNoTrackingAsync(int id,
        CancellationToken cancellationToken = default)
    {
        var groupId = new StudentGroupId(id);
        return await Context.Set<StudentGroup>()
            .AsNoTracking()
            .FirstOrDefaultAsync(group => group.Id == groupId, cancellationToken);
    }


    public async Task<IEnumerable<StudentGroup>> FindByPsychologistIdAsNoTrackingAsync(Guid psychologistId,
        CancellationToken cancellationToken)
    {
        var ownerId = new PsychologistId(psychologistId);
        return await Context.Set<StudentGroup>()
            .AsNoTracking()
            .Where(group => group.PsychologistId == ownerId)
            .OrderBy(group => group.Id)
            .ToListAsync(cancellationToken);
    }


    public async Task<IEnumerable<StudentGroup>> FindByIdsAsNoTrackingAsync(IEnumerable<StudentGroupId> ids,
        CancellationToken cancellationToken)
    {
        var groupIds = ids.ToList();
        return await Context.Set<StudentGroup>()
            .AsNoTracking()
            .Where(group => groupIds.Contains(group.Id))
            .ToListAsync(cancellationToken);
    }


    public async Task<bool> ExistsByNameAndPsychologistIdAsync(string name, Guid psychologistId,
        CancellationToken cancellationToken)
    {
        var ownerId = new PsychologistId(psychologistId);
        return await Context.Set<StudentGroup>()
            .AnyAsync(group => group.PsychologistId == ownerId && group.Name == name, cancellationToken);
    }
}
