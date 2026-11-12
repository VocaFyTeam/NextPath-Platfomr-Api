using VocaFy.NextPath.Platform.Monitoring.Domain.Model.ValueObjects;

namespace VocaFy.NextPath.Platform.Monitoring.Domain.Model.Queries;

public record GetAllVocationalReportsQuery(PsychologistId PsychologistId, StudentId? StudentId = null)
{
    public GetAllVocationalReportsQuery(int psychologistId, int? studentId)
        : this(new PsychologistId(psychologistId), studentId.HasValue ? new StudentId(studentId.Value) : null)
    {
    }
}
