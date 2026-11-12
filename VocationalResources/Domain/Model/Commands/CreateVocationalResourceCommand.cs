namespace VocaFy.NextPath.Platform.VocationalResources.Domain.Model.Commands;

/// <param name="RequesterUserId">Authenticated user; must be a psychologist.</param>
public record CreateVocationalResourceCommand(
    Guid RequesterUserId,
    string Title,
    string Category,
    string? Description,
    string? CoverUrl,
    string FileUrl);
