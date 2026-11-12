namespace VocaFy.NextPath.Platform.VocationalResources.Interfaces.Rest.Resources;

/// <summary>
///     Body for POST and PUT. The psychologist is taken from the token, never from the body.
/// </summary>
public record SaveVocationalResourceResource(
    string Title,
    string Category,
    string? Description,
    string? CoverUrl,
    string FileUrl);
