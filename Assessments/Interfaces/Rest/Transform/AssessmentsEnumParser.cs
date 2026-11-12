using VocaFy.NextPath.Platform.Assessments.Domain.Model.ValueObjects;

namespace VocaFy.NextPath.Platform.Assessments.Interfaces.Rest.Transform;

/// <summary>
///     Converts the lowercase strings used by the frontend ("open", "artistic", "completed") to domain enums
///     and back.
/// </summary>
public static class AssessmentsEnumParser
{
    public static ETestType ToTestType(string? value) =>
        Enum.TryParse<ETestType>(value, true, out var type) ? type : ETestType.Choice;

    public static ETestResultStatus ToStatus(string? value) =>
        Enum.TryParse<ETestResultStatus>(value, true, out var status) ? status : ETestResultStatus.Completed;

    public static EAffinityDimension? ToDimension(string? value) =>
        Enum.TryParse<EAffinityDimension>(value, true, out var dimension) ? dimension : null;

    public static string ToText(Enum value) => value.ToString().ToLowerInvariant();

    /// <summary>Converts {"0193…": "o1"} (question id as text) into a Guid-keyed dictionary. Invalid keys are ignored.</summary>
    public static IReadOnlyDictionary<Guid, string> ToAnswers(IDictionary<string, string>? answers)
    {
        var result = new Dictionary<Guid, string>();
        if (answers is null) return result;
        foreach (var (key, value) in answers)
            if (Guid.TryParse(key, out var questionId)) result[questionId] = value;
        return result;
    }
}
