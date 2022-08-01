using EzCommon.Models;
using EzGym.Events;
using EzGym.Features.Accounts.CreateAccount;
using System;

namespace EzGym.Models
{
    public class Account : AggregateRoot
    {
        public Guid UserId { get; private set; }
        public string AccountName { get; private set; }
        public AccountTypeEnum AccountType { get; private set; }
        public bool IsDefault { get; private set; }
        public string AvatarUrl { get; private set; }

        private Account() { }
        public Account(CreateAccountCommand command)
        {
            RaiseEvent(new AccountCreatedEvent(
                    Id: Guid.NewGuid(),
                    UserId: command.UserId,
                    AccountName: command.AccountName,
                    AccountType: command.AccountType,
                    IsDefault: command.IsDefault
           ));
        }

        public void ChangeAvatarImage(string avatarUrl)
        {
            RaiseEvent(new AvatarImageAccountChanged(Id, avatarUrl));
        }

        protected override void RegisterEvents()
        {
            RegisterEvent<AccountCreatedEvent>(When);
            RegisterEvent<AvatarImageAccountChanged>(When);
        }

        private void When(AvatarImageAccountChanged @event)
        {
            AvatarUrl = @event.AvatarUrl;
        }

        private void When(AccountCreatedEvent @event)
        {
            Id = @event.Id;
            AccountName = @event.AccountName;
            UserId = @event.UserId;
            AccountType = @event.AccountType;
            IsDefault = @event.IsDefault;
        }
    }
}
