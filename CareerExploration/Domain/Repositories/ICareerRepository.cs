using NextPath.Platform.CareerExploration.Domain.Model.Aggregates;
using NextPath.Platform.CareerExploration.Domain.Model.ValueObjects;
using NextPath.Platform.Shared.Domain.Repositories;

namespace NextPath.Platform.CareerExploration.Domain.Repositories;

public interface ICareerRepository : IBaseRepository<Career>
{
    // Named *ByCareerId* on purpose: IBaseRepository.FindByIdAsync takes an int and EF cannot
    // use an int to look up a key mapped as the strongly typed CareerId.
    Task<Career?> FindByCareerIdAsync(CareerId careerId, CancellationToken cancellationToken = default);

    Task<IEnumerable<Career>> FindByCareerIdsAsync(IReadOnlyCollection<CareerId> careerIds,
        CancellationToken cancellationToken = default);

    /// <summary>SQL-side filters (area, modality, duration). Text and university are applied afterwards.</summary>
    Task<IEnumerable<Career>> FindByFiltersAsync(string? area, string? modality, int? durationYears,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken = default);
}
