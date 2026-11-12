using VocaFy.NextPath.Platform.CareerExploration.Application.CommandServices;
using VocaFy.NextPath.Platform.CareerExploration.Domain.Model;
using VocaFy.NextPath.Platform.CareerExploration.Domain.Model.Aggregates;
using VocaFy.NextPath.Platform.CareerExploration.Domain.Model.Commands;
using VocaFy.NextPath.Platform.CareerExploration.Domain.Model.Errors;
using VocaFy.NextPath.Platform.CareerExploration.Domain.Repositories;
using VocaFy.NextPath.Platform.Shared.Application.Model;
using VocaFy.NextPath.Platform.Shared.Domain.Repositories;

namespace VocaFy.NextPath.Platform.CareerExploration.Application.Internal.CommandServices;

public class CareerCommandService(
    ICareerRepository careerRepository,
    IUnitOfWork unitOfWork)
    : ICareerCommandService
{
    public async Task<Result<Career>> Handle(CreateCareerCommand command, CancellationToken cancellationToken)
    {
        if (await careerRepository.ExistsByNameAsync(command.Name.Trim(), cancellationToken))
            return Result<Career>.Failure(CareerExplorationErrors.DuplicateCareerName,
                CareerExplorationError.DuplicateCareerName);

        var career = new Career(command);
        await careerRepository.AddAsync(career, cancellationToken);
        await unitOfWork.CompleteAsync(cancellationToken);
        return Result<Career>.Success(career);
    }
}
