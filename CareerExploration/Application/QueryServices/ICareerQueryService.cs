using NextPath.Platform.CareerExploration.Domain.Model.Aggregates;
using NextPath.Platform.CareerExploration.Domain.Model.Queries;
using NextPath.Platform.CareerExploration.Domain.Model.ValueObjects;
using NextPath.Platform.Shared.Application.Model;

namespace NextPath.Platform.CareerExploration.Application.QueryServices;

public interface ICareerQueryService
{
    Task<IEnumerable<Career>> Handle(GetAllCareersQuery query, CancellationToken cancellationToken);

    Task<Career?> Handle(GetCareerByIdQuery query, CancellationToken cancellationToken);

    Task<IEnumerable<Career>> Handle(GetCareersByIdsQuery query, CancellationToken cancellationToken);

    Task<Result<CareerCompatibility>> Handle(GetCareerCompatibilityQuery query, CancellationToken cancellationToken);
}
