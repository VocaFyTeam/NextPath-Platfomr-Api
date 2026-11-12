using NextPath.Platform.Monitoring.Application.QueryServices;
using NextPath.Platform.Monitoring.Domain.Model.Aggregates;
using NextPath.Platform.Monitoring.Domain.Model.Queries;
using NextPath.Platform.Monitoring.Domain.Repositories;

namespace NextPath.Platform.Monitoring.Application.Internal.QueryServices;

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
