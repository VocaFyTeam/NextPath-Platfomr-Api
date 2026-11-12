namespace VocaFy.NextPath.Platform.Counseling.Domain.Model.ValueObjects;

/// <summary>
///     Student profile snapshot used by GroupSession.SuggestOptimalGrouping: the student and the dominant
///     vocational area of their latest result (artistic, social, investigative...).
/// </summary>
public record GroupingCandidate(StudentId StudentId, string DominantArea);
