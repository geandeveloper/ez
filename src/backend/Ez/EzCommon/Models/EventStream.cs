using EzCommon.Events;

namespace EzCommon.Models
{
    public class EventStream
    {
        public string Id { get; private set; }
        public int Version { get; private set; }
        public IList<EventRow> EventRows { get; private set; }

        public EventStream(EventStreamId id, IReadOnlyList<IEvent> events)
        {
            Id = id.ToString();
            EventRows = events.Select((e) => new EventRow(id, e)).ToList();
            Version = events.Max(e => e.Version);
        }
    }
}
