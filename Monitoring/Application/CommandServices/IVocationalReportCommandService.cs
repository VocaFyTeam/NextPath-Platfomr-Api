using NextPath.Platform.Monitoring.Domain.Model.Aggregates;
using NextPath.Platform.Monitoring.Domain.Model.Commands;
using NextPath.Platform.Shared.Application.Model;

namespace NextPath.Platform.Monitoring.Application.CommandServices;

public interface IVocationalReportCommandService
{
    Task<Result<VocationalReport>> Handle(GenerateVocationalReportCommand command, CancellationToken cancellationToken);
    
    Task<Result<VocationalReport>> Handle(UpdateVocationalReportContentCommand command,
        CancellationToken cancellationToken);
    
    Task<Result<VocationalReport>> Handle(ValidateVocationalReportCommand command, CancellationToken cancellationToken);
}
