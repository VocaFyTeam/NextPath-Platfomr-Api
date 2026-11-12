using VocaFy.NextPath.Platform.Monitoring.Domain.Model.ValueObjects;

namespace VocaFy.NextPath.Platform.Monitoring.Domain.Model.Commands;

public record UpdateVocationalReportContentCommand(
    VocationalReportId VocationalReportId,
    PsychologistId PsychologistId,
    string ContentSummary)
{
    public UpdateVocationalReportContentCommand(int vocationalReportId, Guid psychologistId, string contentSummary)
        : this(new VocationalReportId(vocationalReportId), new PsychologistId(psychologistId), contentSummary)
    {
    }
}
