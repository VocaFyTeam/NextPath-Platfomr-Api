using VocaFy.NextPath.Platform.Shared.Domain.Model.Errors;

namespace VocaFy.NextPath.Platform.Assessments.Domain.Model.Errors;

public static class AssessmentsErrors
{
    public static readonly Error VocationalTestNotFound =
        Error.NotFound("Assessments.VocationalTestNotFound", "The specified vocational test was not found.");

    public static readonly Error TestResultNotFound =
        Error.NotFound("Assessments.TestResultNotFound", "The specified test result was not found.");

    public static readonly Error InvalidAnswers =
        Error.Validation("Assessments.InvalidAnswers", "Some answers do not belong to the vocational test.");

    public static readonly Error TestResultAlreadyCompleted =
        Error.Conflict("Assessments.TestResultAlreadyCompleted", "A completed test result cannot be modified.");

    public static Error DuplicateVocationalTestTitle(string title) =>
        Error.Conflict("Assessments.DuplicateVocationalTestTitle",
            $"A vocational test with the title '{title}' already exists.");
}
