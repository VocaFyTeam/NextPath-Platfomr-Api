using NextPath.Platform.CareerExploration.Domain.Model.Aggregates;
using NextPath.Platform.CareerExploration.Domain.Model.Commands;
using NextPath.Platform.Shared.Application.Model;

namespace NextPath.Platform.CareerExploration.Application.CommandServices;

public interface ICareerCommandService
{
    Task<Result<Career>> Handle(CreateCareerCommand command, CancellationToken cancellationToken);
}
