using NextPath.Platform.Monitoring.Domain.Model.ValueObjects;

namespace NextPath.Platform.Monitoring.Domain.Model.Queries;

public record GetHistoricalProfileByIdQuery(HistoricalProfileId HistoricalProfileId)
{
    public GetHistoricalProfileByIdQuery(int historicalProfileId) : this(new HistoricalProfileId(historicalProfileId))
    {
    }
}
