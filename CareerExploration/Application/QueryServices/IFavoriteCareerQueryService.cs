using NextPath.Platform.CareerExploration.Domain.Model.Aggregates;
using NextPath.Platform.CareerExploration.Domain.Model.Queries;

namespace NextPath.Platform.CareerExploration.Application.QueryServices;

public interface IFavoriteCareerQueryService
{
    Task<IEnumerable<FavoriteCareer>> Handle(GetFavoriteCareersByStudentIdQuery query,
        CancellationToken cancellationToken);

    Task<FavoriteCareer?> Handle(GetFavoriteCareerByIdQuery query, CancellationToken cancellationToken);
}
