using EzCommon.Events;

namespace EzGym.Players;

public record PlayerCreateEvent(string GenerateNewId, string AccountId) : IEvent;