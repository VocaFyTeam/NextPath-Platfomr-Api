using VocaFy.NextPath.Platform.Shared.Domain.Model.Events;

namespace VocaFy.NextPath.Platform.Shared.Application.Internal.EventHandlers;

public interface IEventHandler<in TEvent>
    where TEvent : IEvent{
    Task Handle(TEvent domainEvent, CancellationToken cancellationToken);
}