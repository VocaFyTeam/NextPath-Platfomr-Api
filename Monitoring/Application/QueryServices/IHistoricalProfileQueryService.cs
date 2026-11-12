using VocaFy.NextPath.Platform.Monitoring.Domain.Model.Aggregates;
using VocaFy.NextPath.Platform.Monitoring.Domain.Model.Entities;
using VocaFy.NextPath.Platform.Monitoring.Domain.Model.Queries;
using VocaFy.NextPath.Platform.Monitoring.Domain.Model.ValueObjects;

namespace VocaFy.NextPath.Platform.Monitoring.Application.QueryServices;


public interface IHistoricalProfileQueryService
{
    Task<HistoricalProfile?> Handle(GetHistoricalProfileByIdQuery query, CancellationToken cancellationToken);

    Task<HistoricalProfile?> Handle(GetHistoricalProfileByStudentIdQuery query, CancellationToken cancellationToken);

    Task<IEnumerable<HistoricalProfile>> Handle(GetAllHistoricalProfilesQuery query,
        CancellationToken cancellationToken);
    
    Task<IEnumerable<ProfessionalObservation>?> Handle(GetProfessionalObservationsByHistoricalProfileIdQuery query,
        CancellationToken cancellationToken);
    
    Task<IEnumerable<InterventionRecommendation>?> Handle(GetInterventionRecommendationsByHistoricalProfileIdQuery query,
        CancellationToken cancellationToken);
}
