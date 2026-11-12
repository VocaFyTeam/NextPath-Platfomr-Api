using VocaFy.NextPath.Platform.Monitoring.Domain.Model.ValueObjects;

namespace VocaFy.NextPath.Platform.Monitoring.Domain.Model.Commands;

public record ValidateVocationalReportCommand(VocationalReportId VocationalReportId, PsychologistId PsychologistId)
{
    public ValidateVocationalReportCommand(int vocationalReportId, Guid psychologistId)
        : this(new VocationalReportId(vocationalReportId), new PsychologistId(psychologistId))
    {
    }
}
