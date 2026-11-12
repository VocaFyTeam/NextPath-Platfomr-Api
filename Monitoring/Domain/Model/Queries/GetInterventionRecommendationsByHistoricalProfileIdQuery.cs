using VocaFy.NextPath.Platform.Monitoring.Domain.Model.ValueObjects;

namespace VocaFy.NextPath.Platform.Monitoring.Domain.Model.Queries;

public record GetInterventionRecommendationsByHistoricalProfileIdQuery(HistoricalProfileId HistoricalProfileId)
{
    public GetInterventionRecommendationsByHistoricalProfileIdQuery(int historicalProfileId)
        : this(new HistoricalProfileId(historicalProfileId))
    {
    }
}
