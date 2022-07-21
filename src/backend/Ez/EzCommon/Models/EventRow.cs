using EzCommon.Events;

namespace EzCommon.Models
{
    public class EventRow
    {
        public string Id { get; protected set; }
        public string EventName { get; protected set; }
        public int Version { get; protected set; }
        public IEvent Data { get; protected set; }

        private EventRow() { }

        public EventRow(EventStreamId streamId, IEvent @event)
        {
            Id = new EventRowId(streamId, @event.Version).ToString();
            Version = @event.Version;
            EventName = @event.GetType().Name;
            Data = @event;
        }
    }
}
