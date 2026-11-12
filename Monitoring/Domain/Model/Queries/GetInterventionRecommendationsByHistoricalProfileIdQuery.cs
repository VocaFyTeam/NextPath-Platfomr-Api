using NextPath.Platform.Monitoring.Domain.Model.ValueObjects;

namespace NextPath.Platform.Monitoring.Domain.Model.Queries;

public record GetInterventionRecommendationsByHistoricalProfileIdQuery(HistoricalProfileId HistoricalProfileId)
{
    public GetInterventionRecommendationsByHistoricalProfileIdQuery(int historicalProfileId)
        : this(new HistoricalProfileId(historicalProfileId))
    {
    }
}
