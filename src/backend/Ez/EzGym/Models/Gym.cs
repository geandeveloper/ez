using EzCommon.Models;
using EzGym.Events;
using EzGym.Features.Gyms.CreateGym;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EzGym.Models
{
    public class Gym : AggregateRoot
    {
        public Guid AccountId { get; protected set; }
        public IList<Address> Addresses { get; protected set; }
        public IEnumerable<GymPlan> Plans { get; set; }
        public IEnumerable<GymUser> Users { get; set; }

        private Gym() { }
        public Gym(CreateGymCommand command)
        {
            RaiseEvent(new GymCreatedEvent(
                Id = Guid.NewGuid(),
                AccountId: command.AccountId,
                Addresses: command.Addresses.Select(address => new Address(
                    Cep: address.Cep,
                    Street: address.Street,
                    Number: address.Number,
                    City: address.City,
                    State: address.State,
                    ExtraInformation: address.ExtraInformation)).ToArray()
           ));


        }

        protected void Apply(GymCreatedEvent @event)
        {
            Id = @event.Id;
            AccountId = @event.AccountId;
            Addresses = @event.Addresses;
        }
    }
}
