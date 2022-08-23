using EzCommon.Events;
using System;
using EzGym.Gyms.CreatePlan;

namespace EzGym.Gyms.Events
{
    public record PlanCreatedEvent(Guid Id, CreatePlanCommand Command) : Event;
}
