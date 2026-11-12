using Microsoft.EntityFrameworkCore;
using NextPath.Platform.CareerExploration.Application.CommandServices;
using NextPath.Platform.CareerExploration.Domain.Model;
using NextPath.Platform.CareerExploration.Domain.Model.Aggregates;
using NextPath.Platform.CareerExploration.Domain.Model.Commands;
using NextPath.Platform.CareerExploration.Domain.Model.Errors;
using NextPath.Platform.CareerExploration.Domain.Repositories;
using NextPath.Platform.Shared.Application.Model;
using NextPath.Platform.Shared.Domain.Repositories;

namespace NextPath.Platform.CareerExploration.Application.Internal.CommandServices;

public class FavoriteCareerCommandService(
    IFavoriteCareerRepository favoriteCareerRepository,
    ICareerRepository careerRepository,
    IUnitOfWork unitOfWork)
    : IFavoriteCareerCommandService
{
    public async Task<Result<FavoriteCareer>> Handle(AddFavoriteCareerCommand command,
        CancellationToken cancellationToken)
    {
        var career = await careerRepository.FindByCareerIdAsync(command.CareerId, cancellationToken);
        if (career is null)
            return Result<FavoriteCareer>.Failure(CareerExplorationErrors.CareerNotFound,
                CareerExplorationError.CareerNotFound);

        var existing = await favoriteCareerRepository.FindByStudentIdAndCareerIdAsync(
            command.StudentId, command.CareerId, cancellationToken);
        if (existing is not null)
            return Result<FavoriteCareer>.Failure(CareerExplorationErrors.DuplicateFavoriteCareer,
                CareerExplorationError.DuplicateFavoriteCareer);

        var favorite = new FavoriteCareer(command);
        try
        {
            await favoriteCareerRepository.AddAsync(favorite, cancellationToken);
            await unitOfWork.CompleteAsync(cancellationToken);
            return Result<FavoriteCareer>.Success(favorite);
        }
        catch (DbUpdateException)
        {
            // Two simultaneous requests can both pass the check above; the unique index stops the second one.
            var duplicated = await favoriteCareerRepository.FindByStudentIdAndCareerIdAsync(
                command.StudentId, command.CareerId, cancellationToken);
            return duplicated is not null
                ? Result<FavoriteCareer>.Failure(CareerExplorationErrors.DuplicateFavoriteCareer,
                    CareerExplorationError.DuplicateFavoriteCareer)
                : Result<FavoriteCareer>.Failure(CareerExplorationErrors.DatabaseError,
                    CareerExplorationError.DatabaseError);
        }
    }

    public async Task<Result> Handle(RemoveFavoriteCareerCommand command, CancellationToken cancellationToken)
    {
        var favorite = await favoriteCareerRepository.FindByFavoriteCareerIdAsync(
            command.FavoriteCareerId, cancellationToken);
        if (favorite is null)
            return Result.Failure(CareerExplorationErrors.FavoriteCareerNotFound,
                CareerExplorationError.FavoriteCareerNotFound);

        favoriteCareerRepository.Remove(favorite);
        await unitOfWork.CompleteAsync(cancellationToken);
        return Result.Success();
    }
}
