namespace NextPath.Platform.Monitoring.Interfaces.Acl;

public interface IMonitoringContextFacade
{
    Task<int> CreateHistoricalProfile(int studentId, int psychologistId, int? groupId,
        CancellationToken cancellationToken);

    Task<int> FetchHistoricalProfileIdByStudentId(int studentId, CancellationToken cancellationToken);

    Task<bool> RegisterStudentActivity(int studentId, CancellationToken cancellationToken);
}
