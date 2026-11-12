using VocaFy.NextPath.Platform.Monitoring.Application.QueryServices;
using VocaFy.NextPath.Platform.Monitoring.Domain.Model.Aggregates;
using VocaFy.NextPath.Platform.Monitoring.Domain.Model.Queries;
using VocaFy.NextPath.Platform.Monitoring.Domain.Repositories;

namespace VocaFy.NextPath.Platform.Monitoring.Application.Internal.QueryServices;

public class VocationalReportQueryService(IVocationalReportRepository reportRepository)
    : IVocationalReportQueryService
{

    public async Task<VocationalReport?> Handle(GetVocationalReportByIdQuery query,
        CancellationToken cancellationToken)
    {
        return await reportRepository.FindByIdAsNoTrackingAsync(query.VocationalReportId, cancellationToken);
    }


    public async Task<IEnumerable<VocationalReport>> Handle(GetAllVocationalReportsQuery query,
        CancellationToken cancellationToken)
    {
        int? studentId = query.StudentId is { } student ? student.Value : null;
        return await reportRepository.FindByPsychologistIdAsNoTrackingAsync(query.PsychologistId, studentId,
            cancellationToken);
    }
}
