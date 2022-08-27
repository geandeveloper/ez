using EzCommon.Events;
using EzGym.Gyms.Users.CreateGymUser;

namespace EzGym.Events.Gym
{
    public record GymUserCreateEvent(string Id, CreateGymUserCommand Command) : Event;
}
