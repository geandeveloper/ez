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

        private Account() { }
        public Account(CreateAccountCommand command)
        {
            RaiseEvent(new AccountCreatedEvent(
                    Id: Guid.NewGuid(),
                    UserId: command.UserId,
                    AccountName: command.AccountName,
                    AccountType: command.AccountType
           ));
        }

        protected override void RegisterEvents()
        {
            RegisterEvent<AccountCreatedEvent>(When);
        }

        private void When(AccountCreatedEvent @event)
        {
            Id = @event.Id;
            AccountName = @event.AccountName;
            UserId = @event.UserId;
            AccountType = @event.AccountType;
        }
    }
}
