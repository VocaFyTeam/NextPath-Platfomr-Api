using NextPath.Platform.Monitoring.Domain.Model.ValueObjects;

namespace NextPath.Platform.Monitoring.Domain.Model.Commands;

public record UpdateVocationalReportContentCommand(
    VocationalReportId VocationalReportId,
    PsychologistId PsychologistId,
    string ContentSummary)
{
    public UpdateVocationalReportContentCommand(int vocationalReportId, int psychologistId, string contentSummary)
        : this(new VocationalReportId(vocationalReportId), new PsychologistId(psychologistId), contentSummary)
    {
    }
}
