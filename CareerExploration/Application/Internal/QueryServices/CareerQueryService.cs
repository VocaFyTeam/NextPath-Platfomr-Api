using VocaFy.NextPath.Platform.CareerExploration.Application.Internal.OutboundServices;
using VocaFy.NextPath.Platform.CareerExploration.Application.QueryServices;
using VocaFy.NextPath.Platform.CareerExploration.Domain.Model;
using VocaFy.NextPath.Platform.CareerExploration.Domain.Model.Aggregates;
using VocaFy.NextPath.Platform.CareerExploration.Domain.Model.Errors;
using VocaFy.NextPath.Platform.CareerExploration.Domain.Model.Queries;
using VocaFy.NextPath.Platform.CareerExploration.Domain.Model.ValueObjects;
using VocaFy.NextPath.Platform.CareerExploration.Domain.Repositories;
using VocaFy.NextPath.Platform.Shared.Application.Model;

namespace VocaFy.NextPath.Platform.CareerExploration.Application.Internal.QueryServices;

public class CareerQueryService(
    ICareerRepository careerRepository,
    IAssessmentCompatibilityProvider compatibilityProvider)
    : ICareerQueryService
{
    public async Task<IEnumerable<Career>> Handle(GetAllCareersQuery query, CancellationToken cancellationToken)
    {
        var careers = await careerRepository.FindByFiltersAsync(
            query.Area, query.Modality, query.DurationYears, cancellationToken);

        return careers
            .Where(career => career.IsTaughtAt(query.University) && career.MatchesText(query.Text))
            .ToList();
    }

    public async Task<Career?> Handle(GetCareerByIdQuery query, CancellationToken cancellationToken)
    {
        return await careerRepository.FindByCareerIdAsync(query.CareerId, cancellationToken);
    }

    public async Task<IEnumerable<Career>> Handle(GetCareersByIdsQuery query, CancellationToken cancellationToken)
    {
        return await careerRepository.FindByCareerIdsAsync(query.CareerIds, cancellationToken);
    }

    public async Task<Result<CareerCompatibility>> Handle(GetCareerCompatibilityQuery query,
        CancellationToken cancellationToken)
    {
        var career = await careerRepository.FindByCareerIdAsync(query.CareerId, cancellationToken);
        if (career is null)
            return Result<CareerCompatibility>.Failure(CareerExplorationErrors.CareerNotFound,
                CareerExplorationError.CareerNotFound);

        var percentage = await compatibilityProvider.GetCompatibilityAsync(query.StudentId, career, cancellationToken);
        if (percentage is null)
            return Result<CareerCompatibility>.Failure(CareerExplorationErrors.CompatibilityNotAvailable,
                CareerExplorationError.CompatibilityNotAvailable);

        return Result<CareerCompatibility>.Success(
            new CareerCompatibility(query.CareerId, query.StudentId, percentage.Value));
    }
}
