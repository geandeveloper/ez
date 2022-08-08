using EzCommon.Models;
using EzGym.Events;
using EzGym.Features.Accounts.CreateAccount;
using EzGym.Features.Accounts.UpInsertAccountProfile;
using EzGym.SnapShots;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EzGym.Models
{
    public class Account : AggregateRoot, ISnapShotManager<Account, AccountSnapShot>
    {
        public Guid UserId { get; private set; }
        public string AccountName { get; private set; }
        public AccountTypeEnum AccountType { get; private set; }
        public bool IsDefault { get; private set; }
        public string AvatarUrl { get; private set; }

        public Profile Profile { get; private set; }

        public IList<Follower> Following { get; set; }
        public IList<Follower> Followers { get; set; }

        public int? FollowingCount => Following?.Count;
        public int? FollowersCount => Followers?.Count;

        private Account()
        {
            Followers = new List<Follower>();
            Following = new List<Follower>();
        }

        protected override void RegisterEvents()
        {
            RegisterEvent<AccountCreatedEvent>(When);
            RegisterEvent<AvatarImageAccountChangedEvent>(When);
            RegisterEvent<AccountFollowedEvent>(When);
            RegisterEvent<AddedAccountFollowerEvent>(When);
            RegisterEvent<ProfileChangedEvent>(When);
            RegisterEvent<AccountUnfollowedEvent>(When);
            RegisterEvent<RemovedAccountFollowerEvent>(When);
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
            RaiseEvent(new AddedAccountFollowerEvent(account.Id));
        }
        public void RemoveFollower(Account account)
        {
            RaiseEvent(new RemovedAccountFollowerEvent(account.Id));
        }

        public void FollowAccount(Account account)
        {
            RaiseEvent(new AccountFollowedEvent(account.Id));
        }

        public void UnfollowAccount(Account account)
        {
            RaiseEvent(new AccountUnfollowedEvent(account.Id));
        }

        private void When(ProfileChangedEvent @event)
        {
            Profile = new Profile(
                name: @event.Name,
                jobDescription: @event.JobDescription,
                bioDescription: @event.BioDescription);
        }

        private void When(AccountUnfollowedEvent @event)
        {
            Following.Remove(Following.First(f => f.AccountId == @event.AccountId));
        }

        private void When(RemovedAccountFollowerEvent @event)
        {
            Followers.Remove(Followers.First(f => f.AccountId == @event.AccountId));
        }

        private void When(AddedAccountFollowerEvent @event)
        {
            Followers.Add(new Follower(@event.AccountId));
        }

        private void When(AccountFollowedEvent @event)
        {
            Following.Add(new Follower(@event.AccountId));
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

        public Account FromSnapShot(AccountSnapShot entityState)
        {
            Id = entityState.Id;
            Version = entityState.Version;
            UserId = entityState.UserId;
            AccountName = entityState.AccountName;
            AccountType = entityState.AccountType;
            IsDefault = entityState.IsDefault;
            AvatarUrl = entityState.AvatarUrl;
            Profile = entityState.Profile;
            Following = entityState.Following ?? new List<Follower>();
            Followers = entityState.Followers ?? new List<Follower>();

            return this;
        }

        public AccountSnapShot ToSnapShot()
        {
            return new AccountSnapShot
            {
                Id = Id,
                Version = Version,
                UserId = UserId,
                AccountName = AccountName,
                AccountType = AccountType,
                IsDefault = IsDefault,
                AvatarUrl = AvatarUrl,
                Profile = Profile,
                Following = Following ?? new List<Follower>(),
                Followers = Followers ?? new List<Follower>()
            };
        }

        public static Account RestoreSnapShot(AccountSnapShot snapShot) => new Account().FromSnapShot(snapShot);
    }
}
