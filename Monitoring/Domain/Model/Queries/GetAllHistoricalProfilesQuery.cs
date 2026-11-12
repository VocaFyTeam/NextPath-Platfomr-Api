using NextPath.Platform.Monitoring.Domain.Model.ValueObjects;

namespace NextPath.Platform.Monitoring.Domain.Model.Queries;

public record GetAllHistoricalProfilesQuery(PsychologistId PsychologistId, StudentGroupId? GroupId = null,
    StudentId? StudentId = null)
{
    public GetAllHistoricalProfilesQuery(int psychologistId, int? groupId, int? studentId)
        : this(new PsychologistId(psychologistId),
            groupId.HasValue ? new StudentGroupId(groupId.Value) : null,
            studentId.HasValue ? new StudentId(studentId.Value) : null)
    {
    }
}
