using EzCommon.Events;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EzCommon.Models
{
    public class EventStream
    {
        public string Id { get; protected set; }
        public int Version { get; protected set; } = 0;
        public List<IEvent> Events { get; set; } = new List<IEvent>();

        private EventStream() { }
        public EventStream(EventStreamId id, IReadOnlyList<IEvent> events)
        {
            Id = id.ToString();
            Events = events.ToList();
            Version = events.Any() ? events.Max(e => e.Version) : 0;
        }

        public TEvent GetEvent<TEvent>() where TEvent : class, IEvent
        {
            var eventName = typeof(TEvent).Name;
            var @event = Events.Where(@event => @event.GetType().Name == eventName).FirstOrDefault();
            return @event != null ? @event as TEvent : default;
        }

        public IEnumerable<IEvent> GetUncommitedEvents(int? fromVersion = 0)
        {
            return Events.Where(e => e.Version > (fromVersion ?? 0));
        }
    }
}
