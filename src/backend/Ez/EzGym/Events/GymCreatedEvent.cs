using EzCommon.Events;
using EzGym.Features.Gyms.CreateGym;
using System;

namespace EzGym.Events
{
    public record GymCreatedEvent(Guid Id, CreateGymCommand Command) : Event;
}
