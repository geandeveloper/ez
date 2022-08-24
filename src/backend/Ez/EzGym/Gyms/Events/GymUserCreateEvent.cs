using EzCommon.Events;
using EzGym.Gyms.Users.CreateGymUser;

namespace EzGym.Gyms.Events
{
    public record GymUserCreateEvent(string Id, CreateGymUserCommand Command) : Event;
}
