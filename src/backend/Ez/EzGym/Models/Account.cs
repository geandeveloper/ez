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

        public IList<Follower> Following { get; set; }
        public IList<Follower> Followers { get; set; }

        public int? FollowingCount => Following?.Count;
        public int? FollowersCount => Followers?.Count;

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
            RaiseEvent(new AddedFollowAccountEvent(new Follower(account)));
        }

        public void RemoveFollower(Account account)
        {
            RaiseEvent(new RemovedFollowAccountEvent(new Follower(account)));
        }

        public void FollowAccount(Account account)
        {
            RaiseEvent(new StartFollowAccountEvent(new Follower(account)));
        }

        public void UnfollowAccount(Account account)
        {
            RaiseEvent(new UnfollowAccountEvent(new Follower(account)));
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
            Followers.Remove(@event.Follower);
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
