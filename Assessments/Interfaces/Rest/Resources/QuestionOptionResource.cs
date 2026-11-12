namespace VocaFy.NextPath.Platform.Assessments.Interfaces.Rest.Resources;

/// <summary>Option of a question. "Id" is the option code ("o1", "o2"...).</summary>
public record QuestionOptionResource(string Id, string Text, string Area);
