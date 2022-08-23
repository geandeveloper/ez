using EzCommon.Events;
using System;
using EzGym.Gyms.CreateGym;

namespace EzGym.Gyms.Events
{
    public record GymCreatedEvent(Guid Id, CreateGymCommand Command) : Event;
}
