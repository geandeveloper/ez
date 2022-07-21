using EzCommon.Events;
using System.Collections.Generic;
using System.Linq;

namespace EzCommon.Models
{
    public class EventStream
    {
        public string Id { get; protected set; }
        public int Version { get; protected set; } = 0;
        public List<EventRow> EventRows { get; protected set; }

        private EventStream() { }
        public EventStream(EventStreamId id, IReadOnlyList<IEvent> events)
        {
            Id = id.ToString();
            EventRows = events.Select((e) => new EventRow(id, e)).ToList();
            Version = events.Max(e => e.Version);
        }

        public TEvent GetEvent<TEvent>() where TEvent : class, IEvent
        {
            var eventName = typeof(TEvent).Name;
            var @event = EventRows.Where(@event => @event.EventName == eventName).FirstOrDefault();
            return @event.Data as TEvent;
        }
    }
}
