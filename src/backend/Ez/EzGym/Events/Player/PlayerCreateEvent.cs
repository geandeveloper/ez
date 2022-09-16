using EzCommon.Events;

namespace EzGym.Events.Player;

public record PlayerCreateEvent(string Id, string AccountId) : Event;