namespace VocaFy.NextPath.Platform.Assessments.Interfaces.Rest.Resources;

/// <summary>
///     Same shape the frontend (test-result.assembler.js) already uses.
/// </summary>
public record TestResultResource(
    Guid Id,
    Guid StudentId,
    Guid TestId,
    string Date,
    string Version,
    string Status,
    string Summary,
    ScoresResource Scores,
    IEnumerable<CareerMatchResource> CareerMatches,
    IDictionary<string, string> Answers);
