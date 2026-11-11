using VocaFy.NextPath.Platform.VocationalResources.Domain.Model.Aggregates;
using VocaFy.NextPath.Platform.VocationalResources.Domain.Model.Commands;
using VocaFy.NextPath.Platform.Shared.Application.Model;

namespace VocaFy.NextPath.Platform.VocationalResources.Application.CommandServices;

public interface IVocationalResourceCommandService
{
    Task<Result<VocationalResource>> Handle(CreateVocationalResourceCommand command,
        CancellationToken cancellationToken);

    Task<Result<VocationalResource>> Handle(UpdateVocationalResourceCommand command,
        CancellationToken cancellationToken);

    Task<Result> Handle(DeleteVocationalResourceCommand command, CancellationToken cancellationToken);
}
