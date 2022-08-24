using EzCommon.Events;

namespace EzGym.Gyms.Events;

public record GymUserAssociateWithAccountEvent(string GymUserId, string AccountId) : Event;