namespace VocaFy.NextPath.Platform.Assessments.Interfaces.Rest.Resources;

/// <summary>
///     Body of POST /test-results and PUT /test-results/{id}.
///     Answers: key = question id, value = option code (choice) or free text (open).
///     Scores and summary are NOT received: the backend calculates them.
/// </summary>
public record SubmitTestResultResource(
    Guid StudentId,
    Guid TestId,
    string? Status,
    IDictionary<string, string>? Answers,
    IEnumerable<CareerMatchResource>? CareerMatches);
