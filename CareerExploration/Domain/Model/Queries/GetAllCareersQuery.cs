namespace NextPath.Platform.CareerExploration.Domain.Model.Queries;

/// <summary>Catalog search (HU15 / TS09). Every filter is optional; filters are combined with AND.</summary>
public record GetAllCareersQuery(
    string? Area = null,
    string? Modality = null,
    int? DurationYears = null,
    string? University = null,
    string? Text = null);
