using EzCommon.Events;
using EzGym.Gyms.Users;

namespace EzGym.Gyms.Events;

public record GymMemberShipRegisteredEvent(string Id, GymMemberShip GymMemberShip) : Event;