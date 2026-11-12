using VocaFy.NextPath.Platform.Counseling.Domain.Model.ValueObjects;

namespace VocaFy.NextPath.Platform.Counseling.Domain.Model.Queries;

/// <summary>"Optimal Grouping Analysis" of the event storming.</summary>
public record SuggestOptimalGroupingQuery(IReadOnlyList<GroupingCandidate> Candidates, int MaxParticipants);
