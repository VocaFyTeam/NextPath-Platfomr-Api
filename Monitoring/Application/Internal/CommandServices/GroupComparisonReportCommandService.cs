using NextPath.Platform.Monitoring.Application.CommandServices;
using NextPath.Platform.Monitoring.Application.Internal.OutboundServices;
using NextPath.Platform.Monitoring.Domain.Model;
using NextPath.Platform.Monitoring.Domain.Model.Aggregates;
using NextPath.Platform.Monitoring.Domain.Model.Commands;
using NextPath.Platform.Monitoring.Domain.Model.Errors;
using NextPath.Platform.Monitoring.Domain.Model.ValueObjects;
using NextPath.Platform.Monitoring.Domain.Repositories;
using NextPath.Platform.Shared.Application.Model;
using NextPath.Platform.Shared.Domain.Repositories;

namespace NextPath.Platform.Monitoring.Application.Internal.CommandServices;

public class GroupComparisonReportCommandService(
    IGroupComparisonReportRepository reportRepository,
    IStudentGroupRepository groupRepository,
    IHistoricalProfileRepository profileRepository,
    IAIReportService aiReportService,
    IUnitOfWork unitOfWork)
    : IGroupComparisonReportCommandService
{
    public async Task<Result<GroupComparisonReport>> Handle(GenerateGroupComparisonReportCommand command,
        CancellationToken cancellationToken)
    {
        var groupIds = command.GroupIds.Distinct().ToList();
        if (groupIds.Count < 2)
            return Result<GroupComparisonReport>.Failure(MonitoringErrors.AtLeastTwoGroupsRequired,
                MonitoringError.AtLeastTwoGroupsRequired);

        var groups = (await groupRepository.FindByIdsAsNoTrackingAsync(groupIds, cancellationToken)).ToList();
        if (groups.Count != groupIds.Count)
            return Result<GroupComparisonReport>.Failure(MonitoringErrors.StudentGroupNotFound,
                MonitoringError.StudentGroupNotFound);

        if (groups.Any(group => !group.IsOwnedBy(command.PsychologistId)))
            return Result<GroupComparisonReport>.Failure(MonitoringErrors.PsychologistNotAuthorized,
                MonitoringError.PsychologistNotAuthorized);

        var summaries = new List<GroupMonitoringSummary>();
        foreach (var group in groups)
        {
            var profiles = (await profileRepository.FindByGroupIdAsNoTrackingAsync(group.Id, cancellationToken))
                .ToList();
            summaries.Add(new GroupMonitoringSummary(
                group.Id,
                group.Name,
                profiles.Count,
                profiles.Count(profile => profile.Flags.Contains(EStudentFlag.Inactivity)),
                profiles.Count(profile => profile.Flags.Contains(EStudentFlag.Inconsistent)),
                profiles.Count(profile => profile.Flags.Contains(EStudentFlag.LowParticipation)),
                profiles.Count(profile => profile.Flags.Count > 0)));
        }

        var conclusions = await aiReportService.GenerateAutomaticConclusionsAsync(summaries, cancellationToken);
        if (conclusions.Length > GroupComparisonReport.MaxConclusionsLength)
            conclusions = conclusions[..GroupComparisonReport.MaxConclusionsLength];

        var report = new GroupComparisonReport(command.PsychologistId, groupIds);
        report.CompareStudentGroups(conclusions);
        await reportRepository.AddAsync(report, cancellationToken);
        await unitOfWork.CompleteAsync(cancellationToken);
        return Result<GroupComparisonReport>.Success(report);
    }
}
