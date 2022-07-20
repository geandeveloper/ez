using EzCommon.Events;

namespace EzCommon.Models
{
    public class EventRow
    {
        public string Id { get; private set; }
        public string EventName { get; private set; }
        public int Version { get; private set; }
        public IEvent Data { get; private set; }

        public EventRow(EventStreamId streamId, IEvent @event)
        {
            Id = new EventRowId(streamId, @event.Version).ToString();
            Version = @event.Version;
            EventName = @event.GetType().Name;
            Data = @event;
        }
    }
}
