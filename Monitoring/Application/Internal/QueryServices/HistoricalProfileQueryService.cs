using NextPath.Platform.Monitoring.Application.QueryServices;
using NextPath.Platform.Monitoring.Domain.Model.Aggregates;
using NextPath.Platform.Monitoring.Domain.Model.Entities;
using NextPath.Platform.Monitoring.Domain.Model.Queries;
using NextPath.Platform.Monitoring.Domain.Model.ValueObjects;
using NextPath.Platform.Monitoring.Domain.Repositories;

namespace NextPath.Platform.Monitoring.Application.Internal.QueryServices;


public class HistoricalProfileQueryService(IHistoricalProfileRepository profileRepository)
    : IHistoricalProfileQueryService
{

    public async Task<HistoricalProfile?> Handle(GetHistoricalProfileByIdQuery query,
        CancellationToken cancellationToken)
    {
        return await profileRepository.FindByIdAsNoTrackingAsync(query.HistoricalProfileId, cancellationToken);
    }


    public async Task<HistoricalProfile?> Handle(GetHistoricalProfileByStudentIdQuery query,
        CancellationToken cancellationToken)
    {
        return await profileRepository.FindByStudentIdAsNoTrackingAsync(query.StudentId, cancellationToken);
    }
    
    public async Task<IEnumerable<HistoricalProfile>> Handle(GetAllHistoricalProfilesQuery query,
        CancellationToken cancellationToken)
    {
        int? groupId = query.GroupId is { } group ? group.Value : null;
        int? studentId = query.StudentId is { } student ? student.Value : null;
        return await profileRepository.FindByPsychologistIdAsNoTrackingAsync(query.PsychologistId, groupId, studentId,
            cancellationToken);
    }
    
    public async Task<IEnumerable<ProfessionalObservation>?> Handle(
        GetProfessionalObservationsByHistoricalProfileIdQuery query, CancellationToken cancellationToken)
    {
        var profile = await profileRepository.FindByIdAsNoTrackingAsync(query.HistoricalProfileId, cancellationToken);
        return profile?.Observations.OrderByDescending(observation => observation.CreatedAt).ToList();
    }
    
    public async Task<IEnumerable<InterventionRecommendation>?> Handle(
        GetInterventionRecommendationsByHistoricalProfileIdQuery query, CancellationToken cancellationToken)
    {
        var profile = await profileRepository.FindByIdAsNoTrackingAsync(query.HistoricalProfileId, cancellationToken);
        return profile?.GetInterventionRecommendations();
    }
}
