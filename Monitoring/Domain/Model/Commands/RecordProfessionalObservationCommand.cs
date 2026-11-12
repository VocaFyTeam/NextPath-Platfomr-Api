using VocaFy.NextPath.Platform.Monitoring.Domain.Model.ValueObjects;

namespace VocaFy.NextPath.Platform.Monitoring.Domain.Model.Commands;

public record RecordProfessionalObservationCommand(
    HistoricalProfileId HistoricalProfileId,
    PsychologistId PsychologistId,
    string ObservationText,
    string Agreements,
    DateTimeOffset? SessionDate)
{
    public RecordProfessionalObservationCommand(int historicalProfileId, int psychologistId, string observationText,
        string agreements, DateTimeOffset? sessionDate)
        : this(new HistoricalProfileId(historicalProfileId), new PsychologistId(psychologistId), observationText,
            agreements, sessionDate)
    {
    }
}
