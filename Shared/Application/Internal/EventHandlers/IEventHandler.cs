using VocaFy.NextPath.Platform.Shared.Domain.Model.Events;
using Cortex.Mediator.Notifications;

namespace VocaFy.NextPath.Platform.Shared.Application.Internal.EventHandlers;

public interface IEventHandler<in TEvent> : INotificationHandler<TEvent> where TEvent : IEvent
{
}