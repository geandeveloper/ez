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
        public string FantasyName { get; protected set; }
        public string Cnpj { get; protected set; }
        public IList<Address> Addresses { get; protected set; }

        private Gym() { }
        public Gym(CreateGymCommand command)
        {
            RaiseEvent(new GymCreatedEvent(
                Id = Guid.NewGuid(),
                AccountId: command.AccountId,
                FantasyName: command.FantasyName,
                Cnpj: command.Cnpj,
                Addresses: command.Addresses.Select(address => new Address(
                    Cep: address.Cep,
                    Street: address.Street,
                    Number: address.Number,
                    City: address.City,
                    State: address.State,
                    ExtraInformation: address.ExtraInformation)).ToArray()
           ));


        }

        protected override void RegisterEvents()
        {
            RegisterEvent<GymCreatedEvent>(When);
        }

        private void When(GymCreatedEvent @event)
        {
            Id = @event.Id;
            AccountId = @event.AccountId;
            FantasyName = @event.FantasyName;
            Cnpj = @event.Cnpj;
            Addresses = @event.Addresses;
        }
    }
}
