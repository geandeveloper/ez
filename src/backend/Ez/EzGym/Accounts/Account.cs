using EzCommon.Models;
using EzGym.Accounts.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using EzGym.Accounts.CreateAccount;
using EzGym.Accounts.UpInsertAccountProfile;

namespace EzGym.Accounts
{
    public class Account : AggregateRoot
    {
        public Guid UserId { get; protected set; }
        public string AccountName { get; protected set; }
        public bool IsDefault { get; private set; }
        public string AvatarUrl { get; private set; }
        public Profile Profile { get; private set; }
        public IList<Follower> Following { get; private set; }
        public IList<Follower> Followers { get; private set; }
        public AccountTypeEnum AccountType { get; private set; }

        public int? FollowingCount => Following?.Count;
        public int? FollowersCount => Followers?.Count;

        public Account()
        {
            Followers = new List<Follower>();
            Following = new List<Follower>();
        }

        public Account(CreateAccountCommand command)
        {
            RaiseEvent(new AccountCreatedEvent(
                    Id: Guid.NewGuid(),
                    command
           ));
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

        public void AddFollower(Account account)
        {
            if (!Followers.Select(a => a.AccountId).Contains(account.Id))
                RaiseEvent(new AddedAccountFollowerEvent(account.Id));
        }
        public void RemoveFollower(Account account)
        {
            RaiseEvent(new RemovedAccountFollowerEvent(account.Id));
        }

        public void FollowAccount(Account account)
        {

            if (!Following.Select(a => a.AccountId).Contains(account.Id))
                RaiseEvent(new AccountFollowedEvent(account.Id));
        }

        public void UnfollowAccount(Account account)
        {
            RaiseEvent(new AccountUnfollowedEvent(account.Id));
        }

        protected void Apply(ProfileChangedEvent @event)
        {
            Profile = new Profile(
                name: @event.Name,
                jobDescription: @event.JobDescription,
                bioDescription: @event.BioDescription);
        }


        protected void Apply(AccountUnfollowedEvent @event)
        {
            Following.Remove(Following.First(f => f.AccountId == @event.AccountId));
        }

        protected void Apply(RemovedAccountFollowerEvent @event)
        {
            Followers.Remove(Followers.First(f => f.AccountId == @event.AccountId));
        }

        protected void Apply(AddedAccountFollowerEvent @event)
        {
            Followers.Add(new Follower(@event.AccountId));
        }

        protected void Apply(AccountFollowedEvent @event)
        {
            Following.Add(new Follower(@event.AccountId));
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
