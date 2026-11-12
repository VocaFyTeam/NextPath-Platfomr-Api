using VocaFy.NextPath.Platform.Assessments.Domain.Model.Events;
using VocaFy.NextPath.Platform.Assessments.Domain.Model.ValueObjects;
using VocaFy.NextPath.Platform.Shared.Domain.Model.Aggregates.Internal;

namespace VocaFy.NextPath.Platform.Assessments.Domain.Model.Aggregates;

/// <summary>
///     Attempt of a student on a vocational test. When completed it holds the vocational profile
///     (scores) and the career compatibilities. Aggregate root.
/// </summary>
public partial class TestResult : AggregateRoot
{
    public TestResult()
    {
        Version = string.Empty;
        Summary = string.Empty;
        Status = ETestResultStatus.Pending;
        Scores = new AffinityProfile();
        CareerMatches = new List<CareerMatch>();
        Answers = new List<TestAnswer>();
    }

    public TestResult(Guid studentId, VocationalTest test) : this()
    {
        if (studentId == Guid.Empty) throw new ArgumentException("Student id is required.", nameof(studentId));
        Id = TestResultId.New();
        StudentId = studentId;
        VocationalTestId = test.Id;
        Version = test.Version;
    }

    public TestResultId Id { get; }
    public Guid StudentId { get; private set; }
    public VocationalTestId VocationalTestId { get; private set; }
    public string Version { get; private set; }
    public ETestResultStatus Status { get; private set; }
    public string Summary { get; private set; }
    public AffinityProfile Scores { get; private set; }
    public ICollection<CareerMatch> CareerMatches { get; }
    public ICollection<TestAnswer> Answers { get; }
    public DateTime? CompletedAt { get; private set; }

    public bool IsCompleted => Status == ETestResultStatus.Completed;

    /// <summary>
    ///     Saves partial answers so the student can continue the test later.
    /// </summary>
    public void SaveProgress(IReadOnlyDictionary<Guid, string> answers)
    {
        EnsureNotCompleted();
        ReplaceAnswers(answers);
    }

    /// <summary>
    ///     Completes the test: stores answers, the calculated profile and the career matches.
    /// </summary>
    public void Complete(IReadOnlyDictionary<Guid, string> answers, AffinityProfile scores,
        IEnumerable<CareerMatch> careerMatches)
    {
        EnsureNotCompleted();
        ReplaceAnswers(answers);
        Scores = scores;
        Summary = $"Perfil {scores.HollandCode}";
        CareerMatches.Clear();
        foreach (var match in careerMatches.OrderByDescending(match => match.Compatibility))
            CareerMatches.Add(match);
        Status = ETestResultStatus.Completed;
        CompletedAt = DateTime.UtcNow;
        AddDomainEvent(new TestResultCompletedEvent(Id, StudentId, VocationalTestId, scores.HollandCode));
    }

    private void ReplaceAnswers(IReadOnlyDictionary<Guid, string> answers)
    {
        Answers.Clear();
        foreach (var (questionId, value) in answers) Answers.Add(new TestAnswer(questionId, value));
    }

    private void EnsureNotCompleted()
    {
        if (IsCompleted)
            throw new InvalidOperationException("A completed test result cannot be modified.");
    }
}
