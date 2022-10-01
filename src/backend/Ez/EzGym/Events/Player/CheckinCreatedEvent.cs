
using System;
using EzCommon.Events;

namespace EzGym.Events.Player;

public record CheckInCreatedEvent(string Id, string PlayerId, string GymId, string MemberShipId, DateTime CreateAt) : Event;
