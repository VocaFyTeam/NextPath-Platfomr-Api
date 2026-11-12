using NextPath.Platform.Monitoring.Domain.Model.ValueObjects;

namespace NextPath.Platform.Monitoring.Domain.Model.Queries;

public record GetVocationalReportByIdQuery(VocationalReportId VocationalReportId)
{
    public GetVocationalReportByIdQuery(int vocationalReportId) : this(new VocationalReportId(vocationalReportId))
    {
    }
}
