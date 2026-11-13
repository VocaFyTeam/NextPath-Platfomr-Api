using VocaFy.NextPath.Platform.Assessments.Domain.Model.Commands;
using VocaFy.NextPath.Platform.Assessments.Domain.Model.Entities;
using VocaFy.NextPath.Platform.Assessments.Domain.Model.ValueObjects;
using VocaFy.NextPath.Platform.Shared.Domain.Model.Aggregates.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace VocaFy.NextPath.Platform.Assessments.Domain.Model.Aggregates;

/// <summary>
///     Vocational test of the catalog (NextPath IA, Holland, Kuder...). Aggregate root that owns its questions.
/// </summary>
public class VocationalTest : AggregateRoot
{
    public VocationalTest()
    {
        Title = string.Empty;
        ShortName = string.Empty;
        Headline = string.Empty;
        Description = string.Empty;
        Instructions = string.Empty;
        Version = "1.0";
        Questions = new List<TestQuestion>();
    }

    public VocationalTest(CreateVocationalTestCommand command) : this()
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(command.Title);
        Id = VocationalTestId.New();
        Title = command.Title;
        ShortName = string.IsNullOrWhiteSpace(command.ShortName) ? command.Title : command.ShortName;
        Headline = string.IsNullOrWhiteSpace(command.Headline) ? command.Title.ToUpperInvariant() : command.Headline;
        Description = command.Description;
        Instructions = command.Instructions;
        Type = command.Type;
        Featured = command.Featured;
        Order = command.Order;
        Version = string.IsNullOrWhiteSpace(command.Version) ? "1.0" : command.Version;
        DurationMinutes = command.DurationMinutes;
        foreach (var question in command.Questions) AddQuestion(question);
    }

    public VocationalTestId Id { get; }
    public string Title { get; private set; }
    public string ShortName { get; private set; }
    public string Headline { get; private set; }
    public string Description { get; private set; }
    public string Instructions { get; private set; }
    public ETestType Type { get; private set; }
    public bool Featured { get; private set; }
    public int Order { get; private set; }
    public string Version { get; private set; }
    public int DurationMinutes { get; private set; }
    public ICollection<TestQuestion> Questions { get; }

    [NotMapped]
    public IEnumerable<TestQuestion> OrderedQuestions => Questions.OrderBy(question => question.Order);

    public void AddQuestion(TestQuestionData data)
    {
        var options = data.Options.Select(option => new QuestionOption(option.Code, option.Text, option.Area));
        Questions.Add(new TestQuestion(data.Order, data.Type, data.Text, data.Area, data.Placeholder, options));
    }

    /// <summary>
    ///     Business rule: every answered question must belong to this test.
    /// </summary>
    public bool ContainsAllQuestions(IEnumerable<Guid> questionIds) =>
        questionIds.All(questionId => Questions.Any(question => question.Id.Value == questionId));
}
