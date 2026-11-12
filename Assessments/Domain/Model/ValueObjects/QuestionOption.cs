namespace VocaFy.NextPath.Platform.Assessments.Domain.Model.ValueObjects;

/// <summary>
///     Option of a multiple-choice question. Each option adds points to one affinity dimension.
/// </summary>
public class QuestionOption
{
    public QuestionOption()
    {
        Code = string.Empty;
        Text = string.Empty;
    }

    public QuestionOption(string code, string text, EAffinityDimension area)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(code);
        ArgumentException.ThrowIfNullOrWhiteSpace(text);
        Code = code;
        Text = text;
        Area = area;
    }

    /// <summary>Option code used by the frontend as answer value (e.g. "o1").</summary>
    public string Code { get; private set; }
    public string Text { get; private set; }
    public EAffinityDimension Area { get; private set; }
}
