using EzCommon.Events;
using MediatR;
using System;

namespace EzCommon.EventHandlers;

public interface IEventHandler<TEvent> : INotificationHandler<TEvent>
    where TEvent : IEvent
{
    public string EventName => typeof(TEvent).Name;
    public Type EventType => typeof(TEvent);
}
