using NextPath.Platform.Tasks.Domain.Model.Queries;
using NextPath.Platform.Tasks.Domain.Model.ValueObjects;

namespace NextPath.Platform.Tasks.Application.QueryServices;

public interface IVocationalProgressQueryService
{
    Task<VocationalProgress> Handle(GetVocationalProgressByStudentIdQuery query,
        CancellationToken cancellationToken);
}
