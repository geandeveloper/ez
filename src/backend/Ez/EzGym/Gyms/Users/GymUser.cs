using System;
using System.Collections.Generic;
using EzCommon.Models;
using EzGym.Gyms.Events;
using EzGym.Gyms.Users.CreateGymUser;

namespace EzGym.Gyms.Users
{
    public class GymUser : AggregateRoot
    {
        public string GymId { get; private set; }
        public string Name { get; private set; }
        public string Email { get; private set; }
        public string AccountId { get; private set; }

        public IList<GymMemberShip> MemberShips { get; } = new List<GymMemberShip>();

        public GymUser() { }
        public GymUser(CreateGymUserCommand command)
        {
            RaiseEvent(new GymUserCreateEvent(GenerateNewId(), command));
        }
        public void AddMemberShip(GymMemberShip gymMemberShip)
        {
            RaiseEvent(new GymMemberShipRegisteredEvent(GenerateNewId(), gymMemberShip));
        }

        public void AssociateGymUserWithAccount(string accountId)
        {
            RaiseEvent(new GymUserAssociateWithAccountEvent(Id, accountId));
        }

        protected void Apply(GymMemberShipRegisteredEvent registeredEvent)
        {
            MemberShips.Add(registeredEvent.GymMemberShip);
        }

        protected void Apply(GymUserCreateEvent @event)
        {
            Id = @event.Id;
            GymId = @event.Command.GymId;
            Email = @event.Command.Email;
            Name = @event.Command.Name;
        }

        protected void Apply(GymUserAssociateWithAccountEvent @event)
        {
            AccountId = @event.AccountId;
        }
    }
}
