using VocaFy.NextPath.Platform.Tasks.Domain.Model.Queries;
using VocaFy.NextPath.Platform.Tasks.Domain.Model.ValueObjects;

namespace VocaFy.NextPath.Platform.Tasks.Application.QueryServices;

public interface IVocationalProgressQueryService
{
    Task<VocationalProgress> Handle(GetVocationalProgressByStudentIdQuery query,
        CancellationToken cancellationToken);
}
