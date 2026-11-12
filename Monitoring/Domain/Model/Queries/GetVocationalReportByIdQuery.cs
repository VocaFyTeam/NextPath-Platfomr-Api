using VocaFy.NextPath.Platform.Monitoring.Domain.Model.ValueObjects;

namespace VocaFy.NextPath.Platform.Monitoring.Domain.Model.Queries;

public record GetVocationalReportByIdQuery(VocationalReportId VocationalReportId)
{
    public GetVocationalReportByIdQuery(int vocationalReportId) : this(new VocationalReportId(vocationalReportId))
    {
    }
}
