using NextPath.Platform.Monitoring.Domain.Model.Commands;
using NextPath.Platform.Monitoring.Domain.Model.Events;
using NextPath.Platform.Monitoring.Domain.Model.ValueObjects;
using NextPath.Platform.Shared.Domain.Model.Aggregates.Internal;

namespace NextPath.Platform.Monitoring.Domain.Model.Aggregates;


public partial class StudentGroup : AggregateRoot
{
    public const int MaxNameLength = 50;
    public const int MaxDescriptionLength = 200;
    
    public StudentGroup()
    {
        Name = string.Empty;
        Description = string.Empty;
    }
    
    public StudentGroup(string name, string? description, PsychologistId psychologistId) : this()
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(psychologistId.Value);
        Name = name.Trim();
        Description = description?.Trim() ?? string.Empty;
        PsychologistId = psychologistId;
        AddDomainEvent(new StudentGroupCreatedEvent(Name, psychologistId));
    }
    
    public StudentGroup(CreateStudentGroupCommand command)
        : this(command.Name, command.Description, command.PsychologistId)
    {
    }
    
    public StudentGroupId Id { get; }
    
    public string Name { get; private set; }
    
    public string Description { get; private set; }
    
    public PsychologistId PsychologistId { get; private set; }
    
    public bool IsOwnedBy(PsychologistId psychologistId)
    {
        return PsychologistId == psychologistId;
    }
}
