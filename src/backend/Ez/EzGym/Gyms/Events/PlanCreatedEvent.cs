using EzCommon.Events;
using EzGym.Gyms.CreatePlan;

namespace EzGym.Gyms.Events
{
    public record PlanCreatedEvent(string Id, CreatePlanCommand Command) : Event;
}
