using EzCommon.Models;
using System.Collections.Generic;
using EzGym.Gyms.CreateGym;
using EzGym.Gyms.CreatePlan;
using EzGym.Events.Gym;

namespace EzGym.Gyms
{
    public class Gym : AggregateRoot
    {
        public string AccountId { get; private set; }
        public IList<Address> Addresses { get; protected set; } = new List<Address>();
        public IList<GymPlan> GymPlans { get; set; } = new List<GymPlan>();

        public Gym() { }

        public Gym(CreateGymCommand command) : this()
        {
            RaiseEvent(new GymCreatedEvent(Id = GenerateNewId(), command));
        }

        public void CreatePlan(CreatePlanCommand command)
        {
            RaiseEvent(new PlanCreatedEvent(GenerateNewId(), command));
        }

        protected void Apply(PlanCreatedEvent @event)
        {
            GymPlans.Add(GymPlan.CreateFromEvent(@event));
        }

        protected void Apply(GymCreatedEvent @event)
        {
            Id = @event.Id;
            AccountId = @event.Command.AccountId;
        }
    }
}
