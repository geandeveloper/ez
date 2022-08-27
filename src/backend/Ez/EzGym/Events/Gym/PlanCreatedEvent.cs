using EzCommon.Events;
using EzGym.Gyms.CreatePlan;

namespace EzGym.Events.Gym
{
    public record PlanCreatedEvent(string Id, CreatePlanCommand Command) : Event;
}
