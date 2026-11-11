namespace VocaFy.NextPath.Platform.VocationalResources.Interfaces.Rest.Resources;

/// <param name="UpdatedAt">Last change in yyyy-MM-dd (the format the web app shows).</param>
public record VocationalResourceResource(
    Guid Id,
    Guid PsychologistId,
    string Title,
    string Category,
    string Description,
    string CoverUrl,
    string FileUrl,
    string UpdatedAt);
