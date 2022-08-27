using EzCommon.Events;
using EzGym.Gyms.Users;

namespace EzGym.Events.Gym;

public record GymMemberShipRegisteredEvent(string Id, GymMemberShip GymMemberShip) : Event;