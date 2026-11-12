using VocaFy.NextPath.Platform.Counseling.Domain.Model.Aggregates;
using VocaFy.NextPath.Platform.Counseling.Domain.Model.Commands;
using VocaFy.NextPath.Platform.Shared.Application.Model;

namespace VocaFy.NextPath.Platform.Counseling.Application.CommandServices;

/// <summary>
///     SessionApplicationService of the class diagram (write side).
/// </summary>
public interface ISessionCommandService
{
    Task<Result<Session>> Handle(ScheduleIndividualSessionCommand command, CancellationToken cancellationToken);

    Task<Result<Session>> Handle(ScheduleGroupSessionCommand command, CancellationToken cancellationToken);

    Task<Result<Session>> Handle(UpdateSessionCommand command, CancellationToken cancellationToken);

    Task<Result<Session>> Handle(CancelSessionCommand command, CancellationToken cancellationToken);

    Task<Result<Session>> Handle(CompleteSessionCommand command, CancellationToken cancellationToken);

    Task<Result<Session>> Handle(SaveSessionNotesCommand command, CancellationToken cancellationToken);

    Task<Result<Session>> Handle(AddSessionParticipantCommand command, CancellationToken cancellationToken);
}
