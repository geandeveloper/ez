using System;

namespace EzCommon.Models
{
    public struct EventStreamId
    {
        private readonly Type _streamType;
        private readonly Guid _id;

        public EventStreamId(Type streamType, Guid Id)
        {
            _streamType = streamType;
            _id = Id;
        }

        public override string ToString()
        {
            return $"{_streamType.Name}:{_id}";
        }
    }
}
