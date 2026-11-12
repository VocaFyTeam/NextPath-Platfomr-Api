using VocaFy.NextPath.Platform.Monitoring.Domain.Model.Events;
using VocaFy.NextPath.Platform.Monitoring.Domain.Model.ValueObjects;
using VocaFy.NextPath.Platform.Shared.Domain.Model.Aggregates.Internal;

namespace VocaFy.NextPath.Platform.Monitoring.Domain.Model.Aggregates;

public partial class GroupComparisonReport : AggregateRoot
{
    public const int MaxConclusionsLength = 4000;
    
    public GroupComparisonReport()
    {
        SelectedGroupIds = new List<StudentGroupId>();
        AggregatedConclusions = string.Empty;
    }
    
    public GroupComparisonReport(PsychologistId psychologistId, IEnumerable<StudentGroupId> selectedGroupIds) : this()
    {
        var groups = selectedGroupIds.Distinct().ToList();
        if (groups.Count < 2)
            throw new ArgumentException("At least two different groups are required.", nameof(selectedGroupIds));
        PsychologistId = psychologistId;
        SelectedGroupIds = groups;
    }
    
    public GroupComparisonReportId Id { get; }
    
    public PsychologistId PsychologistId { get; private set; }
    
    public List<StudentGroupId> SelectedGroupIds { get; private set; }
    
    public string AggregatedConclusions { get; private set; }

    public bool IsOwnedBy(PsychologistId psychologistId)
    {
        return PsychologistId == psychologistId;
    }

    public void CompareStudentGroups(string aggregatedConclusions)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(aggregatedConclusions);
        AggregatedConclusions = aggregatedConclusions.Trim();
        AddDomainEvent(new GroupComparisonReportGeneratedEvent(PsychologistId, SelectedGroupIds.Count));
    }
}
