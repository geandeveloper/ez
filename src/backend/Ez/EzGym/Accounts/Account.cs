using EzCommon.Models;
using EzGym.Accounts.Events;
using EzGym.Accounts.CreateAccount;
using EzGym.Accounts.UpInsertAccountProfile;

namespace EzGym.Accounts
{
    public class Account : AggregateRoot
    {
        public string UserId { get; protected set; }
        public string AccountName { get; protected set; }
        public bool IsDefault { get; private set; }
        public string AvatarUrl { get; private set; }
        public Profile Profile { get; private set; }
        public AccountTypeEnum AccountType { get; private set; }

        public Account() { }

        public Account(CreateAccountCommand command)
        {
            RaiseEvent(new AccountCreatedEvent( Id: GenerateNewId(), command));
        }

        public void UpdateProfile(UpInsertAccountProfileCommand command)
        {
            RaiseEvent(new ProfileChangedEvent(
                AccountId: command.AccountId,
                Name: command.Name,
                JobDescription: command.JobDescription,
                BioDescription: command.BioDescription));
        }


        public void ChangeAvatarImage(string avatarUrl)
        {
            RaiseEvent(new AvatarImageAccountChangedEvent(Id, avatarUrl));
        }

        protected void Apply(ProfileChangedEvent @event)
        {
            Profile = new Profile(
                name: @event.Name,
                jobDescription: @event.JobDescription,
                bioDescription: @event.BioDescription);
        }

        protected void Apply(AvatarImageAccountChangedEvent @event)
        {
            AvatarUrl = @event.AvatarUrl;
        }
        protected void Apply(AccountCreatedEvent @event)
        {
            Id = @event.Id;
            AccountName = @event.Command.AccountName;
            AccountType = @event.Command.AccountType;
            UserId = @event.Command.UserId;
            IsDefault = @event.Command.IsDefault;
        }
    }
}
