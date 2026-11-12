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

public class VocationalReportCommandService(
    IVocationalReportRepository reportRepository,
    IHistoricalProfileRepository profileRepository,
    IAIReportService aiReportService,
    IUnitOfWork unitOfWork)
    : IVocationalReportCommandService
{
    public async Task<Result<VocationalReport>> Handle(GenerateVocationalReportCommand command,
        CancellationToken cancellationToken)
    {
        var profile = await profileRepository.FindByStudentIdAsNoTrackingAsync(command.StudentId, cancellationToken);
        if (profile is null)
            return Result<VocationalReport>.Failure(MonitoringErrors.HistoricalProfileNotFound,
                MonitoringError.HistoricalProfileNotFound);

        if (!profile.IsAssignedTo(command.PsychologistId))
            return Result<VocationalReport>.Failure(MonitoringErrors.PsychologistNotAuthorized,
                MonitoringError.PsychologistNotAuthorized);

        var content = command.ReportType switch
        {
            EReportType.Test => await aiReportService.GenerateTestReportAsync(profile, cancellationToken),
            EReportType.IndividualEvolution => await aiReportService.GenerateIndividualEvolutionReportAsync(profile,
                cancellationToken),
            _ => await aiReportService.GeneratePsychologicalReportAsync(profile, cancellationToken)
        };

        if (content.Length > VocationalReport.MaxContentSummaryLength)
            content = content[..VocationalReport.MaxContentSummaryLength];

        var report = new VocationalReport(command.StudentId, command.PsychologistId, command.ReportType, content);
        await reportRepository.AddAsync(report, cancellationToken);
        await unitOfWork.CompleteAsync(cancellationToken);
        return Result<VocationalReport>.Success(report);
    }
    
    public async Task<Result<VocationalReport>> Handle(UpdateVocationalReportContentCommand command,
        CancellationToken cancellationToken)
    {
        var report = await reportRepository.FindByIdAsync(command.VocationalReportId, cancellationToken);
        if (report is null)
            return Result<VocationalReport>.Failure(MonitoringErrors.VocationalReportNotFound,
                MonitoringError.VocationalReportNotFound);

        if (!report.IsOwnedBy(command.PsychologistId))
            return Result<VocationalReport>.Failure(MonitoringErrors.PsychologistNotAuthorized,
                MonitoringError.PsychologistNotAuthorized);

        if (report.IsValidatedByPsychologist)
            return Result<VocationalReport>.Failure(MonitoringErrors.ReportAlreadyValidated,
                MonitoringError.ReportAlreadyValidated);

        if (string.IsNullOrWhiteSpace(command.ContentSummary) ||
            command.ContentSummary.Trim().Length > VocationalReport.MaxContentSummaryLength)
            return Result<VocationalReport>.Failure(MonitoringErrors.InvalidReportContent,
                MonitoringError.InvalidReportContent);

        report.UpdateContentSummary(command.ContentSummary);
        await unitOfWork.CompleteAsync(cancellationToken);
        return Result<VocationalReport>.Success(report);
    }
    
    public async Task<Result<VocationalReport>> Handle(ValidateVocationalReportCommand command,
        CancellationToken cancellationToken)
    {
        var report = await reportRepository.FindByIdAsync(command.VocationalReportId, cancellationToken);
        if (report is null)
            return Result<VocationalReport>.Failure(MonitoringErrors.VocationalReportNotFound,
                MonitoringError.VocationalReportNotFound);

        if (!report.IsOwnedBy(command.PsychologistId))
            return Result<VocationalReport>.Failure(MonitoringErrors.PsychologistNotAuthorized,
                MonitoringError.PsychologistNotAuthorized);

        report.ValidateReport();
        await unitOfWork.CompleteAsync(cancellationToken);
        return Result<VocationalReport>.Success(report);
    }
}
