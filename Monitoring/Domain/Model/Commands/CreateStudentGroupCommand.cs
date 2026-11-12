using VocaFy.NextPath.Platform.Monitoring.Domain.Model.ValueObjects;

namespace VocaFy.NextPath.Platform.Monitoring.Domain.Model.Commands;

public record CreateStudentGroupCommand(string Name, string Description, PsychologistId PsychologistId)
{
    public CreateStudentGroupCommand(string name, string description, int psychologistId)
        : this(name, description, new PsychologistId(psychologistId))
    {
    }
}
