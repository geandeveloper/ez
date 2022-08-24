using EzCommon.Events;
using EzGym.Gyms.CreateGym;

namespace EzGym.Gyms.Events
{
    public record GymCreatedEvent(string Id, CreateGymCommand Command) : Event;
}
