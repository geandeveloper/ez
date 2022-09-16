using EzCommon.Events;

namespace EzGym.Events.Player;

public record PlayerLevelUpEvent(string PlayerId, int CurrentLevel) : Event;