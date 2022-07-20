namespace EzCommon.Models
{
    public struct EventRowId
    {
        private readonly EventStreamId _eventStreamId;
        private readonly int _version;

        public EventRowId(EventStreamId eventStreamId, int version)
        {
            _eventStreamId = eventStreamId;
            _version = version;
        }

        public override string ToString()
        {
            return $"{_version}:{_eventStreamId}";
        }
    }
}
