using VocaFy.NextPath.Platform.CareerExploration.Domain.Model.Aggregates;
using VocaFy.NextPath.Platform.CareerExploration.Domain.Model.Commands;
using VocaFy.NextPath.Platform.Shared.Application.Model;

namespace VocaFy.NextPath.Platform.CareerExploration.Application.CommandServices;

public interface IFavoriteCareerCommandService
{
    Task<Result<FavoriteCareer>> Handle(AddFavoriteCareerCommand command, CancellationToken cancellationToken);

    Task<Result> Handle(RemoveFavoriteCareerCommand command, CancellationToken cancellationToken);
}
