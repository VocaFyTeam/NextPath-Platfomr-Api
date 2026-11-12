using VocaFy.NextPath.Platform.VocationalResources.Domain.Model.Aggregates;
using VocaFy.NextPath.Platform.VocationalResources.Domain.Model.Queries;

namespace VocaFy.NextPath.Platform.VocationalResources.Application.QueryServices;

public interface IVocationalResourceQueryService
{
    Task<VocationalResource?> Handle(GetVocationalResourceByIdQuery query, CancellationToken cancellationToken);

    Task<IEnumerable<VocationalResource>> Handle(GetAllVocationalResourcesQuery query,
        CancellationToken cancellationToken);
}
