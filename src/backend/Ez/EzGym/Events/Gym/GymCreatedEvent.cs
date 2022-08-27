using EzCommon.Events;
using EzGym.Gyms.CreateGym;

namespace EzGym.Events.Gym
{
    public record GymCreatedEvent(string Id, CreateGymCommand Command) : Event;
}
