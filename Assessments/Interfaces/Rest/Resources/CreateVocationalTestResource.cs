namespace VocaFy.NextPath.Platform.Assessments.Interfaces.Rest.Resources;

public record CreateVocationalTestResource(
    string Title,
    string? ShortName,
    string? Headline,
    string Description,
    string Instructions,
    string Type,
    bool Featured,
    int Order,
    string? Version,
    int DurationMinutes,
    IEnumerable<CreateTestQuestionResource>? Questions);
