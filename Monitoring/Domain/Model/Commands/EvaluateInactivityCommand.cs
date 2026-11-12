using NextPath.Platform.Monitoring.Domain.Model.ValueObjects;

namespace NextPath.Platform.Monitoring.Domain.Model.Commands;

public record EvaluateInactivityCommand(PsychologistId PsychologistId)
{
    public EvaluateInactivityCommand(int psychologistId) : this(new PsychologistId(psychologistId))
    {
    }
}
