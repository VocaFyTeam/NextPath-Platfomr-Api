using NextPath.Platform.Monitoring.Domain.Model.Aggregates;
using NextPath.Platform.Monitoring.Domain.Model.Commands;
using NextPath.Platform.Monitoring.Domain.Model.Entities;
using NextPath.Platform.Shared.Application.Model;

namespace NextPath.Platform.Monitoring.Application.CommandServices;

public interface IHistoricalProfileCommandService
{
    Task<Result<HistoricalProfile>> Handle(CreateHistoricalProfileCommand command, CancellationToken cancellationToken);
    
    Task<Result<HistoricalProfile>> Handle(UpdateHistoricalProfileCommand command, CancellationToken cancellationToken);
    
    Task<Result<ProfessionalObservation>> Handle(RecordProfessionalObservationCommand command,
        CancellationToken cancellationToken);
    
    Task<Result<HistoricalProfile>> Handle(RegisterStudentActivityCommand command, CancellationToken cancellationToken);
    
    Task<Result<IEnumerable<HistoricalProfile>>> Handle(EvaluateInactivityCommand command,
        CancellationToken cancellationToken);
}
