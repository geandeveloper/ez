using EzCommon.Events;
using System.Collections.Generic;
using System.Linq;

namespace EzCommon.Models
{
    public class EventStream
    {
        public string Id { get; protected set; }
        public int Version { get; protected set; } = 0;
        private List<EventRow> EventRows { get; set; } = new List<EventRow>();

        private EventStream() { }
        public EventStream(EventStreamId id, IReadOnlyList<IEvent> events)
        {
            Id = id.ToString();
            EventRows = events.Select((e) => new EventRow(id, e)).ToList();
            Version = events.Any() ? events.Max(e => e.Version) : 0;
        }

        public TEvent GetEvent<TEvent>() where TEvent : class, IEvent
        {
            var eventName = typeof(TEvent).Name;
            var @event = EventRows.Where(@event => @event.EventName == eventName).FirstOrDefault();
            return @event  != null ? @event.Data as TEvent : default;
        }

        public IEnumerable<EventRow> GetUncommitedEvents(int? fromVersion = 0)
        {
            return EventRows.Where(e => e.Version > (fromVersion ?? 0));
        }

        public EventStream CommitEvents(IEnumerable<EventRow> eventRows)
        {
            EventRows.AddRange(eventRows.ToList());
            Version = eventRows.Max(e => e.Version);
            return this;
        }
    }
}
