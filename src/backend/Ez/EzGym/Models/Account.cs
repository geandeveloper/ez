using EzCommon.Models;
using EzGym.Events;
using EzGym.Features.Accounts.CreateAccount;
using System;
using System.Collections.Generic;

namespace EzGym.Models
{
    public class Account : AggregateRoot
    {
        public Guid UserId { get; private set; }
        public string AccountName { get; private set; }
        public AccountTypeEnum AccountType { get; private set; }
        public bool IsDefault { get; private set; }
        public string AvatarUrl { get; private set; }
        public IList<Follower> Following { get; private set; }
        public IList<Follower> Followers { get; private set; }

        private Account()
        {
            Followers = new List<Follower>();
            Following = new List<Follower>();
        }

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
            RaiseEvent(new AvatarImageAccountChangedEvent(Id, avatarUrl));
        }

        public void AddFollower(Account account)
        {
            RaiseEvent(new AddedFollowAccountEvent(Id, new Follower(account.Id)));
        }

        public void RemoveFollower(Account account)
        {
            RaiseEvent(new RemovedFollowAccountEvent(Id, new Follower(account.Id)));
        }

        public void FollowAccount(Account account)
        {
            RaiseEvent(new StartFollowAccountEvent(Id, new Follower(account.Id)));
        }

        public void UnfollowAccount(Account account)
        {
            RaiseEvent(new UnfollowAccountEvent(Id, new Follower(account.Id)));
        }

        protected override void RegisterEvents()
        {
            RegisterEvent<AccountCreatedEvent>(When);
            RegisterEvent<AvatarImageAccountChangedEvent>(When);
            RegisterEvent<StartFollowAccountEvent>(When);
            RegisterEvent<UnfollowAccountEvent>(When);
            RegisterEvent<AddedFollowAccountEvent>(When);
            RegisterEvent<RemovedFollowAccountEvent>(When);
        }

        private void When(RemovedFollowAccountEvent @event)
        {
            Followers.Remove(@event.Folower);
        }

        private void When(AddedFollowAccountEvent @event)
        {
            Followers.Add(@event.Follower);
        }

        private void When(UnfollowAccountEvent @event)
        {
            Following.Remove(@event.Follower);
        }

        private void When(StartFollowAccountEvent @event)
        {
            Following.Add(@event.Follower);
        }

        private void When(AvatarImageAccountChangedEvent @event)
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
