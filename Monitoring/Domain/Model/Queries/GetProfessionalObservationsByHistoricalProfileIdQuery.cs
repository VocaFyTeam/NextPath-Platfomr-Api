using NextPath.Platform.Monitoring.Domain.Model.ValueObjects;

namespace NextPath.Platform.Monitoring.Domain.Model.Queries;

public record GetProfessionalObservationsByHistoricalProfileIdQuery(HistoricalProfileId HistoricalProfileId)
{
    public GetProfessionalObservationsByHistoricalProfileIdQuery(int historicalProfileId)
        : this(new HistoricalProfileId(historicalProfileId))
    {
    }
}
