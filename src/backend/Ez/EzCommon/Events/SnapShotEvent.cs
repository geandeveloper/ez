namespace EzCommon.Events {
    public record SnapShotEvent<TSnapShot>(TSnapShot Value) : Event;
}
