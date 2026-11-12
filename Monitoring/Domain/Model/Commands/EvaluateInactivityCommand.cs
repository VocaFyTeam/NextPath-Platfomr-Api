using VocaFy.NextPath.Platform.Monitoring.Domain.Model.ValueObjects;

namespace VocaFy.NextPath.Platform.Monitoring.Domain.Model.Commands;

public record EvaluateInactivityCommand(PsychologistId PsychologistId)
{
    public EvaluateInactivityCommand(int psychologistId) : this(new PsychologistId(psychologistId))
    {
    }
}
