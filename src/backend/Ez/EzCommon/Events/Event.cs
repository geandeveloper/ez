using System;

namespace EzCommon.Events
{
    public record Event : IEvent
    {
        public int Version { get; set; } = 1;
        public DateTime TimeStamp { get; set; } = DateTime.UtcNow;
        public Guid AggregateId { get; set; }
    }
}
