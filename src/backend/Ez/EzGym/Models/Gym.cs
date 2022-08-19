using EzCommon.Models;
using EzGym.Events;
using EzGym.Features.Gyms.CreateGym;
using EzGym.Features.Gyms.CreatePlan;
using System;
using System.Collections.Generic;

namespace EzGym.Models
{
    public class Gym : AggregateRoot
    {
        public Guid UserId { get; protected set; }
        public IList<Address> Addresses { get; protected set; }
        public IList<GymPlan> Plans { get; set; }
        public IList<GymUser> Users { get; set; }

        public Gym()
        {
            Addresses = new List<Address>();
            Plans = new List<GymPlan>();
            Users = new List<GymUser>();
        }

        public Gym(CreateGymCommand command) : this()
        {
            RaiseEvent(new GymCreatedEvent(Id = Guid.NewGuid(), command));
        }

        public void AddPlan(CreatePlanCommand command)
        {
            RaiseEvent(new PlanCreatedEvent(Guid.NewGuid(), command));
        }

        protected void Apply(PlanCreatedEvent @event)
        {
            Plans.Add(new GymPlan(@event.Id, @event.Command.Name, @event.Command.Days, @event.Command.Price, @event.Command.Active));
        }

        protected void Apply(GymCreatedEvent @event)
        {
            Id = @event.Id;
            UserId = @event.Command.UserId;
        }
    }
}
