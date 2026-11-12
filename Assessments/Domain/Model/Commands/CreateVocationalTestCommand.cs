using VocaFy.NextPath.Platform.Assessments.Domain.Model.ValueObjects;

namespace VocaFy.NextPath.Platform.Assessments.Domain.Model.Commands;

/// <summary>
///     Command to register a vocational test with its questions in the catalog.
/// </summary>
public record CreateVocationalTestCommand(
    string Title,
    string ShortName,
    string Headline,
    string Description,
    string Instructions,
    ETestType Type,
    bool Featured,
    int Order,
    string Version,
    int DurationMinutes,
    IEnumerable<TestQuestionData> Questions);

/// <summary>Data of one question inside <see cref="CreateVocationalTestCommand" />.</summary>
public record TestQuestionData(
    int Order,
    ETestType Type,
    string Text,
    EAffinityDimension? Area,
    string? Placeholder,
    IEnumerable<QuestionOptionData> Options);

/// <summary>Data of one option inside <see cref="TestQuestionData" />.</summary>
public record QuestionOptionData(string Code, string Text, EAffinityDimension Area);
