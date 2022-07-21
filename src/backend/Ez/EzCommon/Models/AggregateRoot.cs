using EzCommon.Events;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EzCommon.Models
{
    public abstract class AggregateRoot
    {
        public Guid Id { get; protected set; }
        public int Version { get; protected set; }

        private readonly Queue<IEvent> _events;

        private readonly IDictionary<string, Action<IEvent>> _eventHandlers;

        protected AggregateRoot()
        {
            _events = new Queue<IEvent>();
            _eventHandlers = new Dictionary<string, Action<IEvent>>();
            RegisterEvents();
        }

        protected abstract void RegisterEvents();

        protected void RegisterEvent<TEvent>(Action<TEvent> eventHandler)
            where TEvent : class, IEvent
        {
            _eventHandlers[typeof(TEvent).Name] = @event => eventHandler((TEvent)@event);
        }

        protected void RaiseEvent(IEvent @event)
        {
            @event.Version = Version + 1;
            _eventHandlers[@event.GetType().Name](@event);
            _events.Enqueue(@event);
            Version = _events.Max(e => e.Version);
        }

        public void LoadFromEvents(IEnumerable<IEvent> events)
        {
            foreach (var @event in events)
                RaiseEvent(@event);
        }

        public IEnumerable<IEvent> GetEvents()
        {
            return _events;
        }

        public EventStream ToEventStream()
        {
            var eventStreamId = new EventStreamId(GetType(), Id);
            return new EventStream(eventStreamId, GetEvents().ToList());
        }
    }
}
