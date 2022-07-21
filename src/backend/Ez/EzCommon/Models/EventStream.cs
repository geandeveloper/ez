using EzCommon.Events;
using System.Collections.Generic;
using System.Linq;

namespace EzCommon.Models
{
    public class EventStream
    {
        public string Id { get; private set; }
        public int Version { get; private set; } = 0;
        public IList<EventRow> EventRows { get; private set; }

        public EventStream(EventStreamId id, IReadOnlyList<IEvent> events)
        {
            Id = id.ToString();
            EventRows = events.Select((e) => new EventRow(id, e)).ToList();
            Version = events.Max(e => e.Version);
        }
    }
}
