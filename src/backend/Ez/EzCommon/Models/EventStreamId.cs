using System;

namespace EzCommon.Models
{
    public readonly struct EventStreamId
    {
        private readonly Type _streamType;
        private readonly string _id;

        public EventStreamId(Type streamType, string id)
        {
            _streamType = streamType;
            _id = id;
        }

        public override string ToString()
        {
            return _id;
        }
    }
}
