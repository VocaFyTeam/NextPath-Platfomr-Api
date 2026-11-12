using VocaFy.NextPath.Platform.VocationalResources.Application.QueryServices;
using VocaFy.NextPath.Platform.VocationalResources.Domain.Model.Aggregates;
using VocaFy.NextPath.Platform.VocationalResources.Domain.Model.Queries;
using VocaFy.NextPath.Platform.VocationalResources.Domain.Model.ValueObjects;
using VocaFy.NextPath.Platform.VocationalResources.Domain.Repositories;

namespace VocaFy.NextPath.Platform.VocationalResources.Application.Internal.QueryServices;

public class VocationalResourceQueryService(IVocationalResourceRepository vocationalResourceRepository)
    : IVocationalResourceQueryService
{
    public async Task<VocationalResource?> Handle(GetVocationalResourceByIdQuery query,
        CancellationToken cancellationToken) =>
        await vocationalResourceRepository.FindByIdAsNoTrackingAsync(query.Id, cancellationToken);

    public async Task<IEnumerable<VocationalResource>> Handle(GetAllVocationalResourcesQuery query,
        CancellationToken cancellationToken)
    {
        PsychologistId? psychologistId = query.PsychologistId is { } id ? new PsychologistId(id) : null;
        return await vocationalResourceRepository.FindAllAsync(psychologistId, query.Category, cancellationToken);
    }
}
