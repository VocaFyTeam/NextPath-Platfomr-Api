using VocaFy.NextPath.Platform.Monitoring.Application.CommandServices;
using VocaFy.NextPath.Platform.Monitoring.Application.Internal.OutboundServices.ACL;
using VocaFy.NextPath.Platform.Monitoring.Domain.Model;
using VocaFy.NextPath.Platform.Monitoring.Domain.Model.Aggregates;
using VocaFy.NextPath.Platform.Monitoring.Domain.Model.Commands;
using VocaFy.NextPath.Platform.Monitoring.Domain.Model.Entities;
using VocaFy.NextPath.Platform.Monitoring.Domain.Model.Errors;
using VocaFy.NextPath.Platform.Monitoring.Domain.Model.ValueObjects;
using VocaFy.NextPath.Platform.Monitoring.Domain.Repositories;
using VocaFy.NextPath.Platform.Shared.Application.Model;
using VocaFy.NextPath.Platform.Shared.Domain.Model.Errors;
using VocaFy.NextPath.Platform.Shared.Domain.Repositories;

namespace VocaFy.NextPath.Platform.Monitoring.Application.Internal.CommandServices;

public class HistoricalProfileCommandService(
    IHistoricalProfileRepository profileRepository,
    IStudentGroupRepository groupRepository,
    IExternalIamService externalIamService,
    IUnitOfWork unitOfWork)
    : IHistoricalProfileCommandService
{

    public async Task<Result<HistoricalProfile>> Handle(CreateHistoricalProfileCommand command,
        CancellationToken cancellationToken)
    {
        if (!await externalIamService.ExistsUserByIdAsync(command.StudentId, cancellationToken) ||
            !await externalIamService.ExistsUserByIdAsync(command.PsychologistId, cancellationToken))
            return Result<HistoricalProfile>.Failure(MonitoringErrors.UserNotFound, MonitoringError.UserNotFound);

        if (await profileRepository.ExistsByStudentIdAsync(command.StudentId, cancellationToken))
            return Result<HistoricalProfile>.Failure(
                MonitoringErrors.HistoricalProfileAlreadyExistsForStudent(command.StudentId),
                MonitoringError.HistoricalProfileAlreadyExists);

        if ((command.Observation?.Trim().Length ?? 0) > HistoricalProfile.MaxObservationSummaryLength)
            return Result<HistoricalProfile>.Failure(MonitoringErrors.InvalidObservation,
                MonitoringError.InvalidObservation);

        if (command.GroupId is { } groupId &&
            await CheckGroupAsync(groupId, command.PsychologistId, cancellationToken) is { } failure)
            return Result<HistoricalProfile>.Failure(failure.Error, failure.Code);

        var profile = new HistoricalProfile(command);
        await profileRepository.AddAsync(profile, cancellationToken);
        await unitOfWork.CompleteAsync(cancellationToken);
        return Result<HistoricalProfile>.Success(profile);
    }
    public async Task<Result<HistoricalProfile>> Handle(UpdateHistoricalProfileCommand command,
        CancellationToken cancellationToken)
    {
        var profile = await profileRepository.FindByIdAsync(command.HistoricalProfileId, cancellationToken);
        if (profile is null)
            return Result<HistoricalProfile>.Failure(MonitoringErrors.HistoricalProfileNotFound,
                MonitoringError.HistoricalProfileNotFound);

        if (!profile.IsAssignedTo(command.PsychologistId))
            return Result<HistoricalProfile>.Failure(MonitoringErrors.PsychologistNotAuthorized,
                MonitoringError.PsychologistNotAuthorized);

        if (command.Observation is not null)
        {
            if (command.Observation.Trim().Length > HistoricalProfile.MaxObservationSummaryLength)
                return Result<HistoricalProfile>.Failure(MonitoringErrors.InvalidObservation,
                    MonitoringError.InvalidObservation);
            profile.UpdateObservationSummary(command.Observation);
        }

        if (command.Flags is not null) profile.SetFlags(command.Flags);

        if (command.GroupId is { } groupId)
        {
            if (await CheckGroupAsync(groupId, command.PsychologistId, cancellationToken) is { } failure)
                return Result<HistoricalProfile>.Failure(failure.Error, failure.Code);
            profile.AssignToGroup(groupId);
        }

        await unitOfWork.CompleteAsync(cancellationToken);
        return Result<HistoricalProfile>.Success(profile);
    }
    
    public async Task<Result<ProfessionalObservation>> Handle(RecordProfessionalObservationCommand command,
        CancellationToken cancellationToken)
    {
        var profile = await profileRepository.FindByIdAsync(command.HistoricalProfileId, cancellationToken);
        if (profile is null)
            return Result<ProfessionalObservation>.Failure(MonitoringErrors.HistoricalProfileNotFound,
                MonitoringError.HistoricalProfileNotFound);

        if (!profile.IsAssignedTo(command.PsychologistId))
            return Result<ProfessionalObservation>.Failure(MonitoringErrors.PsychologistNotAuthorized,
                MonitoringError.PsychologistNotAuthorized);

        if (string.IsNullOrWhiteSpace(command.ObservationText) ||
            command.ObservationText.Trim().Length > ProfessionalObservation.MaxObservationLength ||
            (command.Agreements?.Trim().Length ?? 0) > ProfessionalObservation.MaxAgreementsLength)
            return Result<ProfessionalObservation>.Failure(MonitoringErrors.InvalidObservation,
                MonitoringError.InvalidObservation);

        var observation = profile.RecordObservation(command.PsychologistId, command.ObservationText,
            command.Agreements, command.SessionDate);
        await unitOfWork.CompleteAsync(cancellationToken);
        return Result<ProfessionalObservation>.Success(observation);
    }
    
    public async Task<Result<HistoricalProfile>> Handle(RegisterStudentActivityCommand command,
        CancellationToken cancellationToken)
    {
        var profile = await profileRepository.FindByStudentIdAsync(command.StudentId, cancellationToken);
        if (profile is null)
            return Result<HistoricalProfile>.Failure(MonitoringErrors.HistoricalProfileNotFound,
                MonitoringError.HistoricalProfileNotFound);

        profile.RegisterActivity(command.ActivityDate ?? DateTimeOffset.UtcNow);
        await unitOfWork.CompleteAsync(cancellationToken);
        return Result<HistoricalProfile>.Success(profile);
    }
    
    public async Task<Result<IEnumerable<HistoricalProfile>>> Handle(EvaluateInactivityCommand command,
        CancellationToken cancellationToken)
    {
        var profiles = (await profileRepository.FindByPsychologistIdAsync(command.PsychologistId, cancellationToken))
            .ToList();
        var now = DateTimeOffset.UtcNow;
        foreach (var profile in profiles) profile.RefreshInactivityFlag(now);

        await unitOfWork.CompleteAsync(cancellationToken);
        return Result<IEnumerable<HistoricalProfile>>.Success(profiles);
    }

    private async Task<(Error Error, MonitoringError Code)?> CheckGroupAsync(StudentGroupId groupId,
        PsychologistId psychologistId, CancellationToken cancellationToken)
    {
        var group = await groupRepository.FindByIdAsNoTrackingAsync(groupId, cancellationToken);
        if (group is null)
            return (MonitoringErrors.StudentGroupNotFound, MonitoringError.StudentGroupNotFound);
        if (!group.IsOwnedBy(psychologistId))
            return (MonitoringErrors.PsychologistNotAuthorized, MonitoringError.PsychologistNotAuthorized);
        return null;
    }
}
