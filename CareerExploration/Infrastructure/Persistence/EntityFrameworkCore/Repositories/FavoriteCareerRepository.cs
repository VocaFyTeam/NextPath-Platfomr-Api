using Microsoft.EntityFrameworkCore;
using NextPath.Platform.CareerExploration.Domain.Model.Aggregates;
using NextPath.Platform.CareerExploration.Domain.Model.ValueObjects;
using NextPath.Platform.CareerExploration.Domain.Repositories;
using NextPath.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using NextPath.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

namespace NextPath.Platform.CareerExploration.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class FavoriteCareerRepository(AppDbContext context)
    : BaseRepository<FavoriteCareer>(context), IFavoriteCareerRepository
{
    public async Task<FavoriteCareer?> FindByFavoriteCareerIdAsync(FavoriteCareerId favoriteCareerId,
        CancellationToken cancellationToken = default)
    {
        return await Context.Set<FavoriteCareer>()
            .FirstOrDefaultAsync(favorite => favorite.Id == favoriteCareerId, cancellationToken);
    }

    public async Task<FavoriteCareer?> FindByFavoriteCareerIdAsNoTrackingAsync(FavoriteCareerId favoriteCareerId,
        CancellationToken cancellationToken = default)
    {
        return await Context.Set<FavoriteCareer>()
            .AsNoTracking()
            .FirstOrDefaultAsync(favorite => favorite.Id == favoriteCareerId, cancellationToken);
    }

    public async Task<IEnumerable<FavoriteCareer>> FindByStudentIdAsync(StudentId studentId,
        CancellationToken cancellationToken = default)
    {
        return await Context.Set<FavoriteCareer>()
            .AsNoTracking()
            .Where(favorite => favorite.StudentId == studentId)
            .OrderBy(favorite => favorite.Id)
            .ToListAsync(cancellationToken);
    }

    public async Task<FavoriteCareer?> FindByStudentIdAndCareerIdAsync(StudentId studentId, CareerId careerId,
        CancellationToken cancellationToken = default)
    {
        return await Context.Set<FavoriteCareer>()
            .AsNoTracking()
            .FirstOrDefaultAsync(
                favorite => favorite.StudentId == studentId && favorite.CareerId == careerId,
                cancellationToken);
    }

    public async Task<int> CountByStudentIdAsync(StudentId studentId, CancellationToken cancellationToken = default)
    {
        return await Context.Set<FavoriteCareer>()
            .CountAsync(favorite => favorite.StudentId == studentId, cancellationToken);
    }
}
