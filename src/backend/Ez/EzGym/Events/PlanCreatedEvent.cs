using EzCommon.Events;
using EzGym.Features.Gyms.CreatePlan;
using System;

namespace EzGym.Events
{
    public record PlanCreatedEvent(Guid Id, CreatePlanCommand Command) : Event;
}
