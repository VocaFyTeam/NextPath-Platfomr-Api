using VocaFy.NextPath.Platform.Shared.Domain.Model.Events;
namespace VocaFy.NextPath.Platform.Shared.Domain.Model.Aggregates;

public interface IAggregateRoot
{
    IReadOnlyCollection<IEvent> DomainEvents {get;}
    void ClearDomainEvents();
}