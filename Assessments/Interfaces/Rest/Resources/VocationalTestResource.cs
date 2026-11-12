namespace VocaFy.NextPath.Platform.Assessments.Interfaces.Rest.Resources;

public record VocationalTestResource(
    Guid Id,
    string Title,
    string ShortName,
    string Headline,
    string Description,
    string Instructions,
    string Type,
    bool Featured,
    int Order,
    string Version,
    int DurationMinutes);
