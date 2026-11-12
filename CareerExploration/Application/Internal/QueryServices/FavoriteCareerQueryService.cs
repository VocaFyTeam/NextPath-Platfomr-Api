using VocaFy.NextPath.Platform.CareerExploration.Application.QueryServices;
using VocaFy.NextPath.Platform.CareerExploration.Domain.Model.Aggregates;
using VocaFy.NextPath.Platform.CareerExploration.Domain.Model.Queries;
using VocaFy.NextPath.Platform.CareerExploration.Domain.Repositories;

namespace VocaFy.NextPath.Platform.CareerExploration.Application.Internal.QueryServices;

public class FavoriteCareerQueryService(IFavoriteCareerRepository favoriteCareerRepository)
    : IFavoriteCareerQueryService
{
    public async Task<IEnumerable<FavoriteCareer>> Handle(GetFavoriteCareersByStudentIdQuery query,
        CancellationToken cancellationToken)
    {
        return await favoriteCareerRepository.FindByStudentIdAsync(query.StudentId, cancellationToken);
    }

    public async Task<FavoriteCareer?> Handle(GetFavoriteCareerByIdQuery query, CancellationToken cancellationToken)
    {
        return await favoriteCareerRepository.FindByFavoriteCareerIdAsNoTrackingAsync(
            query.FavoriteCareerId, cancellationToken);
    }
}
