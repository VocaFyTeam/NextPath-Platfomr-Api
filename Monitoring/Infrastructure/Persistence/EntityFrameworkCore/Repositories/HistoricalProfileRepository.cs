using NextPath.Platform.Monitoring.Domain.Model.Aggregates;
using NextPath.Platform.Monitoring.Domain.Model.ValueObjects;
using NextPath.Platform.Monitoring.Domain.Repositories;
using NextPath.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using NextPath.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Microsoft.EntityFrameworkCore;

namespace NextPath.Platform.Monitoring.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class HistoricalProfileRepository(AppDbContext context) : BaseRepository<HistoricalProfile>(context), IHistoricalProfileRepository
{
    
    public new async Task<HistoricalProfile?> FindByIdAsync(int id, CancellationToken cancellationToken)
    {
        var profileId = new HistoricalProfileId(id);
        return await Context.Set<HistoricalProfile>()
            .Include(profile => profile.Observations)
            .FirstOrDefaultAsync(profile => profile.Id == profileId, cancellationToken);
    }
    
    public new async Task<HistoricalProfile?> FindByIdAsNoTrackingAsync(int id,
        CancellationToken cancellationToken = default)
    {
        var profileId = new HistoricalProfileId(id);
        return await Context.Set<HistoricalProfile>()
            .AsNoTracking()
            .AsSplitQuery()
            .Include(profile => profile.Observations)
            .FirstOrDefaultAsync(profile => profile.Id == profileId, cancellationToken);
    }
    
    public async Task<HistoricalProfile?> FindByStudentIdAsync(int studentId, CancellationToken cancellationToken)
    {
        var id = new StudentId(studentId);
        return await Context.Set<HistoricalProfile>()
            .Include(profile => profile.Observations)
            .FirstOrDefaultAsync(profile => profile.StudentId == id, cancellationToken);
    }
    public async Task<HistoricalProfile?> FindByStudentIdAsNoTrackingAsync(int studentId,
        CancellationToken cancellationToken)
    {
        var id = new StudentId(studentId);
        return await Context.Set<HistoricalProfile>()
            .AsNoTracking()
            .AsSplitQuery()
            .Include(profile => profile.Observations)
            .FirstOrDefaultAsync(profile => profile.StudentId == id, cancellationToken);
    }
    
    public async Task<bool> ExistsByStudentIdAsync(int studentId, CancellationToken cancellationToken)
    {
        var id = new StudentId(studentId);
        return await Context.Set<HistoricalProfile>().AnyAsync(profile => profile.StudentId == id, cancellationToken);
    }
    public async Task<IEnumerable<HistoricalProfile>> FindByPsychologistIdAsNoTrackingAsync(int psychologistId,
        int? groupId, int? studentId, CancellationToken cancellationToken)
    {
        var ownerId = new PsychologistId(psychologistId);
        var query = Context.Set<HistoricalProfile>()
            .AsNoTracking()
            .Where(profile => profile.AssignedPsychologistId == ownerId);

        if (groupId.HasValue)
        {
            var filterGroupId = new StudentGroupId(groupId.Value);
            query = query.Where(profile => profile.GroupId == filterGroupId);
        }

        if (studentId.HasValue)
        {
            var filterStudentId = new StudentId(studentId.Value);
            query = query.Where(profile => profile.StudentId == filterStudentId);
        }

        return await query.OrderBy(profile => profile.Id).ToListAsync(cancellationToken);
    }
    
    public async Task<IEnumerable<HistoricalProfile>> FindByPsychologistIdAsync(int psychologistId,
        CancellationToken cancellationToken)
    {
        var ownerId = new PsychologistId(psychologistId);
        return await Context.Set<HistoricalProfile>()
            .Where(profile => profile.AssignedPsychologistId == ownerId)
            .OrderBy(profile => profile.Id)
            .ToListAsync(cancellationToken);
    }
    
    public async Task<IEnumerable<HistoricalProfile>> FindByGroupIdAsNoTrackingAsync(int groupId,
        CancellationToken cancellationToken)
    {
        var filterGroupId = new StudentGroupId(groupId);
        return await Context.Set<HistoricalProfile>()
            .AsNoTracking()
            .Where(profile => profile.GroupId == filterGroupId)
            .OrderBy(profile => profile.Id)
            .ToListAsync(cancellationToken);
    }
}
