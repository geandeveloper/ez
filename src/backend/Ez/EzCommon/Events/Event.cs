namespace EzCommon.Events
{
    public class Event : IEvent
    {
        public int Version { get; set; } = 1;
        public DateTime TimeStamp { get; set; } = DateTime.UtcNow;
    }
}
