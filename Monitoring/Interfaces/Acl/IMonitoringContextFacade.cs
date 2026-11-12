namespace VocaFy.NextPath.Platform.Monitoring.Interfaces.Acl;

public interface IMonitoringContextFacade
{
    Task<int> CreateHistoricalProfile(Guid studentId, Guid psychologistId, int? groupId,
        CancellationToken cancellationToken);

    Task<int> FetchHistoricalProfileIdByStudentId(Guid studentId, CancellationToken cancellationToken);

    Task<bool> RegisterStudentActivity(Guid studentId, CancellationToken cancellationToken);
}
