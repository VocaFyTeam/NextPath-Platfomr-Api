using VocaFy.NextPath.Platform.Monitoring.Domain.Model.Aggregates;
using VocaFy.NextPath.Platform.Monitoring.Domain.Model.ValueObjects;
using VocaFy.NextPath.Platform.Monitoring.Domain.Repositories;
using VocaFy.NextPath.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using VocaFy.NextPath.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Microsoft.EntityFrameworkCore;

namespace VocaFy.NextPath.Platform.Monitoring.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class VocationalReportRepository(AppDbContext context) : BaseRepository<VocationalReport>(context), IVocationalReportRepository
{

    public async Task<VocationalReport?> FindByIdAsync(int id, CancellationToken cancellationToken)
    {
        var reportId = new VocationalReportId(id);
        return await Context.Set<VocationalReport>()
            .FirstOrDefaultAsync(report => report.Id == reportId, cancellationToken);
    }

 
    public async Task<VocationalReport?> FindByIdAsNoTrackingAsync(int id,
        CancellationToken cancellationToken = default)
    {
        var reportId = new VocationalReportId(id);
        return await Context.Set<VocationalReport>()
            .AsNoTracking()
            .FirstOrDefaultAsync(report => report.Id == reportId, cancellationToken);
    }


    public async Task<IEnumerable<VocationalReport>> FindByPsychologistIdAsNoTrackingAsync(Guid psychologistId,
        Guid? studentId, CancellationToken cancellationToken)
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
