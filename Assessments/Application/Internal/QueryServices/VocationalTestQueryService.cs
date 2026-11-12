using VocaFy.NextPath.Platform.Assessments.Application.QueryServices;
using VocaFy.NextPath.Platform.Assessments.Domain.Model.Aggregates;
using VocaFy.NextPath.Platform.Assessments.Domain.Model.Entities;
using VocaFy.NextPath.Platform.Assessments.Domain.Model.Queries;
using VocaFy.NextPath.Platform.Assessments.Domain.Repositories;

namespace VocaFy.NextPath.Platform.Assessments.Application.Internal.QueryServices;

public class VocationalTestQueryService(IVocationalTestRepository vocationalTestRepository)
    : IVocationalTestQueryService
{
    public async Task<IEnumerable<VocationalTest>> Handle(GetAllVocationalTestsQuery query,
        CancellationToken cancellationToken)
    {
        return await vocationalTestRepository.ListOrderedAsNoTrackingAsync(cancellationToken);
    }

    public async Task<VocationalTest?> Handle(GetVocationalTestByIdQuery query, CancellationToken cancellationToken)
    {
        return await vocationalTestRepository.FindByTestIdAsNoTrackingAsync(query.VocationalTestId,
            cancellationToken);
    }

    public async Task<IEnumerable<TestQuestion>> Handle(GetTestQuestionsByVocationalTestIdQuery query,
        CancellationToken cancellationToken)
    {
        var vocationalTest = await vocationalTestRepository.FindWithQuestionsByIdAsNoTrackingAsync(
            query.VocationalTestId, cancellationToken);
        return vocationalTest?.OrderedQuestions ?? Enumerable.Empty<TestQuestion>();
    }
}
