using VocaFy.NextPath.Platform.Assessments.Domain.Model.ValueObjects;

namespace VocaFy.NextPath.Platform.Assessments.Domain.Model.Commands;

/// <summary>
///     Command to continue (or finish) a pending attempt.
/// </summary>
public record UpdateTestResultCommand(
    Guid TestResultId,
    ETestResultStatus Status,
    IReadOnlyDictionary<Guid, string> Answers,
    IEnumerable<CareerMatch> CareerMatches);
