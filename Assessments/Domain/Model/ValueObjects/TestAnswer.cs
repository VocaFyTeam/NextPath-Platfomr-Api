namespace VocaFy.NextPath.Platform.Assessments.Domain.Model.ValueObjects;

/// <summary>
///     Answer given by the student to one question: an option code (choice) or free text (open).
/// </summary>
public class TestAnswer
{
    public TestAnswer()
    {
        Value = string.Empty;
    }

    public TestAnswer(Guid questionId, string value)
    {
        QuestionId = questionId;
        Value = value ?? string.Empty;
    }

    public Guid QuestionId { get; private set; }
    public string Value { get; private set; }
}
