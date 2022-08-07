using EzCommon.Models;
using EzGym.Events;
using EzGym.Features.Accounts.CreateAccount;
using EzGym.Features.Accounts.UpInsertAccountProfile;
using EzGym.SnapShots;
using System;
using System.Collections.Generic;

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
        protected override void RegisterEvents()
        {
            RegisterEvent<AccountCreatedEvent>(When);
            RegisterEvent<AvatarImageAccountChangedEvent>(When);
            RegisterEvent<AccountFollowedEvent>(When);
            RegisterEvent<AddedAccountFollowerEvent>(When);
            RegisterEvent<ProfileChangedEvent>(When);
        }


        public void UpdateProfile(UpInsertAccountProfileCommand command)
        {
            RaiseEvent(new ProfileChangedEvent(
                AccountId: command.AccountId,
                Name: command.Name,
                JobDescription: command.JobDescription,
                BioDescription: command.BioDescription));
        }

        private void When(ProfileChangedEvent @event)
        {
            Profile = new Profile(
                name: @event.Name,
                jobDescription: @event.JobDescription,
                bioDescription: @event.BioDescription);
        }

        public void ChangeAvatarImage(string avatarUrl)
        {
            RaiseEvent(new AvatarImageAccountChangedEvent(Id, avatarUrl));
        }

        public void AddFollower(Account account)
        {
            RaiseEvent(new AddedAccountFollowerEvent(account.Id));
        }

        public void FollowAccount(Account account)
        {
            RaiseEvent(new AccountFollowedEvent(account.Id));
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
            Following = entityState.Following;
            Followers = entityState.Followers;

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
                Following = Following,
                Followers = Followers,
            };
        }

        public static Account RestoreSnapShot(AccountSnapShot snapShot) => new Account().FromSnapShot(snapShot);
    }
}
