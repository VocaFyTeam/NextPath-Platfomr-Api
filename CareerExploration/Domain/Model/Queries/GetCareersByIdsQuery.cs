using NextPath.Platform.CareerExploration.Domain.Model.ValueObjects;

namespace NextPath.Platform.CareerExploration.Domain.Model.Queries;

/// <summary>Careers selected for the side-by-side comparison (HU18, maximum 3).</summary>
public record GetCareersByIdsQuery(IReadOnlyList<CareerId> CareerIds);
