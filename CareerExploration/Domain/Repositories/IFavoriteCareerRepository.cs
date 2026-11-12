using VocaFy.NextPath.Platform.CareerExploration.Domain.Model.Aggregates;
using VocaFy.NextPath.Platform.CareerExploration.Domain.Model.ValueObjects;
using VocaFy.NextPath.Platform.Shared.Domain.Repositories;

namespace VocaFy.NextPath.Platform.CareerExploration.Domain.Repositories;

public interface IFavoriteCareerRepository : IBaseRepository<FavoriteCareer>
{
    /// <summary>Tracked entity, used by commands that modify or remove it.</summary>
    Task<FavoriteCareer?> FindByFavoriteCareerIdAsync(FavoriteCareerId favoriteCareerId,
        CancellationToken cancellationToken = default);

    Task<FavoriteCareer?> FindByFavoriteCareerIdAsNoTrackingAsync(FavoriteCareerId favoriteCareerId,
        CancellationToken cancellationToken = default);

    Task<IEnumerable<FavoriteCareer>> FindByStudentIdAsync(StudentId studentId,
        CancellationToken cancellationToken = default);

    Task<FavoriteCareer?> FindByStudentIdAndCareerIdAsync(StudentId studentId, CareerId careerId,
        CancellationToken cancellationToken = default);

    Task<int> CountByStudentIdAsync(StudentId studentId, CancellationToken cancellationToken = default);
}
