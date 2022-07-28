using EzCommon.Events;
using MediatR;

namespace EzCommon.EventHandlers;

public interface IEventHandler<TEvent> : INotificationHandler<TEvent>
    where TEvent : IEvent
{
}
