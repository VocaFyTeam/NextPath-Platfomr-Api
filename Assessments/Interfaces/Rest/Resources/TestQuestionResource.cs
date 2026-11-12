namespace VocaFy.NextPath.Platform.Assessments.Interfaces.Rest.Resources;

public record TestQuestionResource(
    Guid Id,
    Guid TestId,
    int Order,
    string Type,
    string Text,
    string? Area,
    string Placeholder,
    IEnumerable<QuestionOptionResource> Options);
