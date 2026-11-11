namespace VocaFy.NextPath.Platform.VocationalResources.Domain.Model.Queries;

/// <summary>
///     Resource library, newest first. Both filters are optional.
/// </summary>
public record GetAllVocationalResourcesQuery(Guid? PsychologistId = null, string? Category = null);
