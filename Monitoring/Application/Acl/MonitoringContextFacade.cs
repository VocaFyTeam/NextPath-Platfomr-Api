using VocaFy.NextPath.Platform.Monitoring.Application.CommandServices;
using VocaFy.NextPath.Platform.Monitoring.Application.QueryServices;
using VocaFy.NextPath.Platform.Monitoring.Domain.Model.Commands;
using VocaFy.NextPath.Platform.Monitoring.Domain.Model.Queries;
using VocaFy.NextPath.Platform.Monitoring.Interfaces.Acl;

namespace VocaFy.NextPath.Platform.Monitoring.Application.Acl;

public class MonitoringContextFacade(
    IHistoricalProfileCommandService profileCommandService,
    IHistoricalProfileQueryService profileQueryService) : IMonitoringContextFacade
{
    public async Task<int> CreateHistoricalProfile(Guid studentId, Guid psychologistId, int? groupId,
        CancellationToken cancellationToken)
    {
        var command = new CreateHistoricalProfileCommand(studentId, psychologistId, groupId, string.Empty);
        var result = await profileCommandService.Handle(command, cancellationToken);
        return result.Value?.Id.Value ?? 0;
    }
    
    public async Task<int> FetchHistoricalProfileIdByStudentId(Guid studentId, CancellationToken cancellationToken)
    {
        var profile = await profileQueryService.Handle(new GetHistoricalProfileByStudentIdQuery(studentId),
            cancellationToken);
        return profile?.Id.Value ?? 0;
    }
    
    public async Task<bool> RegisterStudentActivity(Guid studentId, CancellationToken cancellationToken)
    {
        var result = await profileCommandService.Handle(new RegisterStudentActivityCommand(studentId, null),
            cancellationToken);
        return result.IsSuccess;
    }
}
