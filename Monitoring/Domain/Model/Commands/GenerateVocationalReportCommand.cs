using VocaFy.NextPath.Platform.Monitoring.Domain.Model.ValueObjects;

namespace VocaFy.NextPath.Platform.Monitoring.Domain.Model.Commands;

public record GenerateVocationalReportCommand(StudentId StudentId, PsychologistId PsychologistId, EReportType ReportType)
{
    public GenerateVocationalReportCommand(Guid studentId, Guid psychologistId, EReportType reportType)
        : this(new StudentId(studentId), new PsychologistId(psychologistId), reportType)
    {
    }
}
