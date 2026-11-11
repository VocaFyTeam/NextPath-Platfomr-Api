using VocaFy.NextPath.Platform.Iam.Domain.Model.Aggregates;
using VocaFy.NextPath.Platform.Iam.Domain.Model.Queries;

namespace VocaFy.NextPath.Platform.Iam.Application.QueryServices;

public interface IPsychologistQueryService
{
    Task<Psychologist?> Handle(GetPsychologistByIdQuery query, CancellationToken cancellationToken);
    Task<Psychologist?> Handle(GetPsychologistByUserIdQuery query, CancellationToken cancellationToken);
    Task<IEnumerable<Psychologist>> Handle(GetAllPsychologistsQuery query, CancellationToken cancellationToken);
}
