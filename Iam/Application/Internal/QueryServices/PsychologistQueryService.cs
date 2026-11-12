using VocaFy.NextPath.Platform.Iam.Application.QueryServices;
using VocaFy.NextPath.Platform.Iam.Domain.Model.Aggregates;
using VocaFy.NextPath.Platform.Iam.Domain.Model.Queries;
using VocaFy.NextPath.Platform.Iam.Domain.Repositories;

namespace VocaFy.NextPath.Platform.Iam.Application.Internal.QueryServices;

public class PsychologistQueryService(IPsychologistRepository psychologistRepository) : IPsychologistQueryService
{
    public async Task<Psychologist?> Handle(GetPsychologistByIdQuery query, CancellationToken cancellationToken) =>
        await psychologistRepository.FindByIdAsNoTrackingAsync(query.Id, cancellationToken);

    public async Task<Psychologist?> Handle(GetPsychologistByUserIdQuery query,
        CancellationToken cancellationToken) =>
        await psychologistRepository.FindByUserIdAsync(query.UserId, cancellationToken);

    public async Task<IEnumerable<Psychologist>> Handle(GetAllPsychologistsQuery query,
        CancellationToken cancellationToken) =>
        await psychologistRepository.ListAsNoTrackingAsync(cancellationToken);
}
