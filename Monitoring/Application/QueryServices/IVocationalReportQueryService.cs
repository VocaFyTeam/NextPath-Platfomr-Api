using NextPath.Platform.Monitoring.Domain.Model.Aggregates;
using NextPath.Platform.Monitoring.Domain.Model.Queries;

namespace NextPath.Platform.Monitoring.Application.QueryServices;

public interface IVocationalReportQueryService
{
    Task<VocationalReport?> Handle(GetVocationalReportByIdQuery query, CancellationToken cancellationToken);

    Task<IEnumerable<VocationalReport>> Handle(GetAllVocationalReportsQuery query,
        CancellationToken cancellationToken);
}
