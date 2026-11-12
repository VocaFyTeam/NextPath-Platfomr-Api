using NextPath.Platform.Monitoring.Domain.Model.Aggregates;
using NextPath.Platform.Monitoring.Domain.Model.ValueObjects;
using NextPath.Platform.Monitoring.Domain.Repositories;
using NextPath.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using NextPath.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Microsoft.EntityFrameworkCore;

namespace NextPath.Platform.Monitoring.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class VocationalReportRepository(AppDbContext context) : BaseRepository<VocationalReport>(context), IVocationalReportRepository
{

    public new async Task<VocationalReport?> FindByIdAsync(int id, CancellationToken cancellationToken)
    {
        var reportId = new VocationalReportId(id);
        return await Context.Set<VocationalReport>()
            .FirstOrDefaultAsync(report => report.Id == reportId, cancellationToken);
    }

 
    public new async Task<VocationalReport?> FindByIdAsNoTrackingAsync(int id,
        CancellationToken cancellationToken = default)
    {
        var reportId = new VocationalReportId(id);
        return await Context.Set<VocationalReport>()
            .AsNoTracking()
            .FirstOrDefaultAsync(report => report.Id == reportId, cancellationToken);
    }


    public async Task<IEnumerable<VocationalReport>> FindByPsychologistIdAsNoTrackingAsync(int psychologistId,
        int? studentId, CancellationToken cancellationToken)
    {
        var ownerId = new PsychologistId(psychologistId);
        var query = Context.Set<VocationalReport>()
            .AsNoTracking()
            .Where(report => report.PsychologistId == ownerId);

        if (studentId.HasValue)
        {
            var filterStudentId = new StudentId(studentId.Value);
            query = query.Where(report => report.StudentId == filterStudentId);
        }

        return await query.OrderBy(report => report.Id).ToListAsync(cancellationToken);
    }
}
