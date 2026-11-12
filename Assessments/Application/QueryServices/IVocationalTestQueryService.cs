using VocaFy.NextPath.Platform.Assessments.Domain.Model.Aggregates;
using VocaFy.NextPath.Platform.Assessments.Domain.Model.Entities;
using VocaFy.NextPath.Platform.Assessments.Domain.Model.Queries;

namespace VocaFy.NextPath.Platform.Assessments.Application.QueryServices;

public interface IVocationalTestQueryService
{
    Task<IEnumerable<VocationalTest>> Handle(GetAllVocationalTestsQuery query, CancellationToken cancellationToken);

    Task<VocationalTest?> Handle(GetVocationalTestByIdQuery query, CancellationToken cancellationToken);

    Task<IEnumerable<TestQuestion>> Handle(GetTestQuestionsByVocationalTestIdQuery query,
        CancellationToken cancellationToken);
}
