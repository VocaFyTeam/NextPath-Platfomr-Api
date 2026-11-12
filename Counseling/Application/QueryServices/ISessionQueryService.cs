using VocaFy.NextPath.Platform.Counseling.Domain.Model.Aggregates;
using VocaFy.NextPath.Platform.Counseling.Domain.Model.Queries;
using VocaFy.NextPath.Platform.Counseling.Domain.Model.ValueObjects;
using VocaFy.NextPath.Platform.Shared.Application.Model;

namespace VocaFy.NextPath.Platform.Counseling.Application.QueryServices;

/// <summary>
///     SessionApplicationService of the class diagram (read side).
/// </summary>
public interface ISessionQueryService
{
    Task<Session?> Handle(GetSessionByIdQuery query, CancellationToken cancellationToken);

    Task<IEnumerable<Session>> Handle(GetAllSessionsQuery query, CancellationToken cancellationToken);

    Result<IReadOnlyList<GroupingSuggestion>> Handle(SuggestOptimalGroupingQuery query);
}
