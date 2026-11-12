using VocaFy.NextPath.Platform.Assessments.Domain.Model.ValueObjects;

namespace VocaFy.NextPath.Platform.Assessments.Domain.Model.Entities;

/// <summary>
///     Question that belongs to a <see cref="Aggregates.VocationalTest" />.
/// </summary>
public class TestQuestion
{
    public TestQuestion()
    {
        Text = string.Empty;
        Placeholder = string.Empty;
        Options = new List<QuestionOption>();
    }

    public TestQuestion(int order, ETestType type, string text, EAffinityDimension? area, string? placeholder,
        IEnumerable<QuestionOption> options) : this()
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(text);
        Id = TestQuestionId.New();
        Order = order;
        Type = type;
        Text = text;
        Area = area;
        Placeholder = placeholder ?? string.Empty;
        foreach (var option in options) Options.Add(option);
    }

    public TestQuestionId Id { get; }
    public VocationalTestId VocationalTestId { get; private set; }
    public int Order { get; private set; }
    public ETestType Type { get; private set; }
    public string Text { get; private set; }
    public EAffinityDimension? Area { get; private set; }
    public string Placeholder { get; private set; }
    public ICollection<QuestionOption> Options { get; }

    public bool IsOpen => Type == ETestType.Open;

    /// <summary>
    ///     Business rule: an open answer needs at least 3 characters; a choice answer must be one of the options.
    /// </summary>
    public bool IsAnswered(string? answer)
    {
        if (IsOpen) return !string.IsNullOrWhiteSpace(answer) && answer.Trim().Length >= 3;
        return Options.Any(option => option.Code == answer);
    }
}
