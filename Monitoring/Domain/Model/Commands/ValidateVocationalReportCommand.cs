using NextPath.Platform.Monitoring.Domain.Model.ValueObjects;

namespace NextPath.Platform.Monitoring.Domain.Model.Commands;

public record ValidateVocationalReportCommand(VocationalReportId VocationalReportId, PsychologistId PsychologistId)
{
    public ValidateVocationalReportCommand(int vocationalReportId, int psychologistId)
        : this(new VocationalReportId(vocationalReportId), new PsychologistId(psychologistId))
    {
    }
}
