namespace VocaFy.NextPath.Platform.Counseling.Domain.Model.ValueObjects;

/// <summary>
///     One suggested group: the shared criteria (dominant area, or "mixed") and its students.
/// </summary>
public record GroupingSuggestion(string Criteria, IReadOnlyList<StudentId> StudentIds);
