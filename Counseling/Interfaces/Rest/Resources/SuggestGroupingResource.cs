namespace VocaFy.NextPath.Platform.Counseling.Interfaces.Rest.Resources;

/// <param name="StudentId">Student to group.</param>
/// <param name="DominantArea">Dominant area of the student's latest result (artistic, social, investigative...).</param>
public record GroupingCandidateResource(Guid StudentId, string DominantArea);

/// <param name="MaxParticipants">Size limit of each suggested group (2 to 30).</param>
public record SuggestGroupingResource(int MaxParticipants, IReadOnlyList<GroupingCandidateResource> Students);

public record GroupingSuggestionResource(string Criteria, IReadOnlyList<Guid> StudentIds);
