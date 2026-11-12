using VocaFy.NextPath.Platform.Assessments.Domain.Model.ValueObjects;

namespace VocaFy.NextPath.Platform.Assessments.Domain.Model.Commands;

/// <summary>
///     Command to register a new attempt of a student. Status "Completed" calculates the profile;
///     status "Pending" only saves the answers.
/// </summary>
public record SubmitTestResultCommand(
    Guid StudentId,
    Guid VocationalTestId,
    ETestResultStatus Status,
    IReadOnlyDictionary<Guid, string> Answers,
    IEnumerable<CareerMatch> CareerMatches);
