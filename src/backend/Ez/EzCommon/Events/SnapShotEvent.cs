using EzCommon.Models;

namespace EzCommon.Events
{
    public record SnapShotEvent<T>(T SnapShot) : Event;
}
