using VocaFy.NextPath.Platform.Counseling.Domain.Model.Entities;
using VocaFy.NextPath.Platform.Counseling.Domain.Model.Events;
using VocaFy.NextPath.Platform.Counseling.Domain.Model.ValueObjects;

namespace VocaFy.NextPath.Platform.Counseling.Domain.Model.Aggregates;

/// <summary>
///     Group feedback / orientation session with several students (US05, US28).
/// </summary>
public class GroupSession : Session
{
    public const int MinParticipants = 2;
    public const int MaxAllowedParticipants = 30;
    public const string MixedCriteria = "mixed";

    private readonly List<SessionParticipant> _participants = [];

    public GroupSession()
    {
    }

    public GroupSession(PsychologistId hostId, IEnumerable<StudentId> attendeeIds, string? studentGroupId,
        string? criteria, int? participantsLimit, string sessionTitle, DateTime sessionDateTime,
        int sessionDuration, string? sessionNotes, string? sessionMeetLink)
        : base(hostId, sessionTitle, sessionDateTime, sessionDuration, sessionNotes, sessionMeetLink)
    {
        var attendees = attendeeIds.Distinct().ToList();
        UpdateGroupInfo(studentGroupId, criteria, participantsLimit ?? MaxAllowedParticipants, attendees.Count);
        foreach (var attendee in attendees) _participants.Add(new SessionParticipant(attendee));
        EnsureEnoughParticipants();

        AddDomainEvent(new GroupSessionScheduledEvent(Id, PsychologistId,
            _participants.Select(p => p.StudentId.Value).ToList(), ScheduledDateTime));
    }

    /// <summary>Optional reference to a student group of the psychologist (e.g. "A", "B").</summary>
    public string? GroupId { get; private set; }

    public string? GroupingCriteria { get; private set; }
    public int MaxParticipants { get; private set; }

    public IReadOnlyCollection<SessionParticipant> Participants => _participants.AsReadOnly();

    public override ESessionType Type => ESessionType.Group;

    public override IReadOnlyCollection<StudentId> StudentIds =>
        _participants.Select(p => p.StudentId).ToList().AsReadOnly();

    public bool IsFull => _participants.Count >= MaxParticipants;

    public bool HasParticipant(StudentId studentId) => _participants.Any(p => p.StudentId == studentId);

    public static bool IsValidParticipantsCount(int count, int maxParticipants) =>
        count >= MinParticipants && count <= maxParticipants && maxParticipants <= MaxAllowedParticipants;

    public GroupSession UpdateGroupInfo(string? groupId, string? groupingCriteria, int maxParticipants)
    {
        EnsureScheduled();
        return UpdateGroupInfo(groupId, groupingCriteria, maxParticipants, _participants.Count);
    }

    /// <summary>addParticipant() of the diagram.</summary>
    /// <returns>False when the student is already in the session or the session is full.</returns>
    public bool AddParticipant(StudentId studentId)
    {
        EnsureScheduled();
        if (HasParticipant(studentId) || IsFull) return false;
        _participants.Add(new SessionParticipant(studentId));
        return true;
    }

    /// <summary>
    ///     Keeps the students that stay, removes the ones that left and adds the new ones
    ///     (diff instead of clear + add so EF Core does not track two participants with the same key).
    /// </summary>
    public GroupSession ReplaceParticipants(IEnumerable<StudentId> studentIds)
    {
        EnsureScheduled();
        var target = studentIds.Distinct().ToList();
        if (!IsValidParticipantsCount(target.Count, MaxParticipants))
            throw new ArgumentException(
                $"A group session needs between {MinParticipants} and {MaxParticipants} students.",
                nameof(studentIds));

        _participants.RemoveAll(p => !target.Contains(p.StudentId));
        foreach (var studentId in target.Where(id => !HasParticipant(id)))
            _participants.Add(new SessionParticipant(studentId));
        return this;
    }

    /// <summary>
    ///     suggestOptimalGrouping() of the diagram: students that share their dominant vocational area go
    ///     together, in groups of at most <paramref name="maxParticipants" />. Areas with a single student are
    ///     pooled into "mixed" groups so nobody is left alone.
    /// </summary>
    public static IReadOnlyList<GroupingSuggestion> SuggestOptimalGrouping(
        IEnumerable<GroupingCandidate> studentProfiles, int maxParticipants)
    {
        var candidates = studentProfiles
            .Where(c => !string.IsNullOrWhiteSpace(c.DominantArea))
            .DistinctBy(c => c.StudentId)
            .ToList();
        if (candidates.Count < MinParticipants)
            throw new InvalidOperationException("At least two student profiles are needed to suggest groups.");
        if (maxParticipants < MinParticipants || maxParticipants > MaxAllowedParticipants)
            throw new ArgumentOutOfRangeException(nameof(maxParticipants));

        var suggestions = new List<GroupingSuggestion>();
        var leftovers = new List<StudentId>();

        foreach (var area in candidates.GroupBy(c => c.DominantArea.Trim().ToLowerInvariant())
                     .OrderByDescending(g => g.Count()))
        {
            foreach (var chunk in area.Select(c => c.StudentId).Chunk(maxParticipants))
                if (chunk.Length >= MinParticipants) suggestions.Add(new GroupingSuggestion(area.Key, chunk));
                else leftovers.AddRange(chunk);
        }

        if (leftovers.Count >= MinParticipants)
            suggestions.AddRange(leftovers.Chunk(maxParticipants)
                .Select(chunk => new GroupingSuggestion(MixedCriteria, chunk)));
        else if (leftovers.Count == 1)
            AttachToSmallestGroup(suggestions, leftovers[0], maxParticipants);

        return suggestions;
    }

    private static void AttachToSmallestGroup(List<GroupingSuggestion> suggestions, StudentId studentId,
        int maxParticipants)
    {
        var index = suggestions.FindIndex(s =>
            s.StudentIds.Count == suggestions.Min(x => x.StudentIds.Count) && s.StudentIds.Count < maxParticipants);
        if (index < 0)
        {
            var last = suggestions[^1];
            if (last.StudentIds.Count - 1 < MinParticipants)
            {
                // Groups of two that are all full: the psychologist decides where this student goes.
                suggestions.Add(new GroupingSuggestion(MixedCriteria, [studentId]));
                return;
            }

            // Every group is full: open a new mixed group taking one student from the last one.
            suggestions[^1] = last with { StudentIds = last.StudentIds.Take(last.StudentIds.Count - 1).ToList() };
            suggestions.Add(new GroupingSuggestion(MixedCriteria, [last.StudentIds[^1], studentId]));
            return;
        }

        var target = suggestions[index];
        suggestions[index] = new GroupingSuggestion(MixedCriteria, [..target.StudentIds, studentId]);
    }

    private GroupSession UpdateGroupInfo(string? groupId, string? groupingCriteria, int maxParticipants,
        int currentParticipants)
    {
        if (maxParticipants < MinParticipants || maxParticipants > MaxAllowedParticipants ||
            maxParticipants < currentParticipants)
            throw new ArgumentOutOfRangeException(nameof(maxParticipants), maxParticipants,
                $"The limit must be between {MinParticipants} and {MaxAllowedParticipants} and fit the current students.");

        GroupId = string.IsNullOrWhiteSpace(groupId) ? null : groupId.Trim();
        GroupingCriteria = string.IsNullOrWhiteSpace(groupingCriteria) ? null : groupingCriteria.Trim();
        MaxParticipants = maxParticipants;
        return this;
    }

    private void EnsureEnoughParticipants()
    {
        if (_participants.Count < MinParticipants)
            throw new ArgumentException($"A group session needs at least {MinParticipants} students.");
    }
}
