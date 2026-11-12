using VocaFy.NextPath.Platform.CareerExploration.Domain.Model.Aggregates;
using VocaFy.NextPath.Platform.CareerExploration.Domain.Model.Commands;
using VocaFy.NextPath.Platform.Shared.Application.Model;

namespace VocaFy.NextPath.Platform.CareerExploration.Application.CommandServices;

public interface ICareerCommandService
{
    Task<Result<Career>> Handle(CreateCareerCommand command, CancellationToken cancellationToken);
}
