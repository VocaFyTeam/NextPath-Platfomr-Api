namespace VocaFy.NextPath.Platform.VocationalResources.Domain.Model.Commands;

/// <param name="RequesterUserId">Authenticated user; must be the psychologist who owns the resource.</param>
public record UpdateVocationalResourceCommand(
    Guid VocationalResourceId,
    Guid RequesterUserId,
    string Title,
    string Category,
    string? Description,
    string? CoverUrl,
    string FileUrl);
