using EzCommon.Events;

namespace EzGym.Events.Gym;

public record GymUserAssociateWithAccountEvent(string GymUserId, string AccountId) : Event;