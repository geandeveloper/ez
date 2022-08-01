using EzCommon.Models;
using EzGym.Events;
using EzGym.Features.Profiles.UpInsertProfile;
using System;

namespace EzGym.Models
{
    public class Profile : AggregateRoot
    {
        public Guid AccountId { get; private set; }
        public string Name { get; private set; }
        public string JobDescription { get; private set; }
        public string BioDescription { get; private set; }


        private Profile() { }

        public Profile(UpInsertProfileCommand command)
        {
            RaiseEvent(new ProfileChangedEvent(
                AccountId: command.AccountId,
                Name: command.Name,
                JobDescription: command.JobDescription,
                BioDescription: command.BioDescription));

        }

        public void UpdateProfile(UpInsertProfileCommand command)
        {
            RaiseEvent(new ProfileChangedEvent(
                AccountId: command.AccountId,
                Name: command.Name,
                JobDescription: command.JobDescription,
                BioDescription: command.BioDescription));
        }

        protected override void RegisterEvents()
        {
            RegisterEvent<ProfileChangedEvent>(When);
        }

        private void When(ProfileChangedEvent @event)
        {
            Id = @event.AccountId;
            AccountId = @event.AccountId;
            Name = @event.Name;
            JobDescription = @event.JobDescription;
            BioDescription = @event.BioDescription;
        }
    }
}
