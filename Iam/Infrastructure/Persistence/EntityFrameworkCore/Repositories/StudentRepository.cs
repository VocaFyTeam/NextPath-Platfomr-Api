using VocaFy.NextPath.Platform.Iam.Domain.Model.Aggregates;
using VocaFy.NextPath.Platform.Iam.Domain.Model.ValueObjects;
using VocaFy.NextPath.Platform.Iam.Domain.Repositories;
using VocaFy.NextPath.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using VocaFy.NextPath.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Microsoft.EntityFrameworkCore;

namespace VocaFy.NextPath.Platform.Iam.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class StudentRepository(AppDbContext context) : BaseRepository<Student>(context), IStudentRepository
{
    public async Task<Student?> FindByIdAsNoTrackingAsync(StudentId id, CancellationToken cancellationToken) =>
        await Context.Set<Student>().AsNoTracking().FirstOrDefaultAsync(s => s.Id == id, cancellationToken);

    public async Task<Student?> FindByUserIdAsync(UserId userId, CancellationToken cancellationToken) =>
        await Context.Set<Student>().AsNoTracking().FirstOrDefaultAsync(s => s.UserId == userId, cancellationToken);

    public async Task<IEnumerable<Student>> FindByLinkedPsychologistCodeAsync(string linkingCode,
        CancellationToken cancellationToken) =>
        await Context.Set<Student>().AsNoTracking()
            .Where(s => s.LinkedPsychologistCode == linkingCode)
            .ToListAsync(cancellationToken);
}
