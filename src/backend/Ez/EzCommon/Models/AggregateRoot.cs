using EzCommon.Events;
using ReflectionMagic;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EzCommon.Models
{
    public abstract class AggregateRoot
    {

        private static readonly int SnapShotInterval = 1;

        public Guid Id { get; protected set; }
        public int Version { get; protected set; }

        private readonly Queue<IEvent> _events;


        protected AggregateRoot()
        {
            _events = new Queue<IEvent>();
        }

        protected void RaiseEvent(IEvent @event)
        {
            @event.Version = Version + 1;
            Version = @event.Version;

            this.AsDynamic().Apply(@event);

            _events.Enqueue(@event);
            @event.AggregateId = Id;
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
