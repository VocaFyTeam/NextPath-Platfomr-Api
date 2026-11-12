using NextPath.Platform.CareerExploration.Domain.Model.Aggregates;
using NextPath.Platform.CareerExploration.Domain.Model.Commands;
using NextPath.Platform.Shared.Application.Model;

namespace NextPath.Platform.CareerExploration.Application.CommandServices;

public interface IFavoriteCareerCommandService
{
    Task<Result<FavoriteCareer>> Handle(AddFavoriteCareerCommand command, CancellationToken cancellationToken);

    Task<Result> Handle(RemoveFavoriteCareerCommand command, CancellationToken cancellationToken);
}
