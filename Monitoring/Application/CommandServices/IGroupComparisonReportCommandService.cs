using NextPath.Platform.Monitoring.Domain.Model.Aggregates;
using NextPath.Platform.Monitoring.Domain.Model.Commands;
using NextPath.Platform.Shared.Application.Model;

namespace NextPath.Platform.Monitoring.Application.CommandServices;

public interface IGroupComparisonReportCommandService
{
    Task<Result<GroupComparisonReport>> Handle(GenerateGroupComparisonReportCommand command,
        CancellationToken cancellationToken);
}
