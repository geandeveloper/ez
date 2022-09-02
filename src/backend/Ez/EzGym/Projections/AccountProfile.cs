using EzGym.Accounts;
using EzGym.Accounts.Events;
using Marten.Events.Aggregation;

namespace EzGym.Projections
{
    public class AccountProfile
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string AccountName { get; set; }
        public bool IsDefault { get; set; }
        public string AvatarUrl { get; set; }
        public Profile Profile { get; set; }
        public AccountTypeEnum AccountType { get; set; }
        public long FollowersCount { get; set; }
        public long FollowingCount { get; set; }
    }

    public class AccountProfileProjection : SingleStreamAggregation<AccountProfile>
    {
        public AccountProfileProjection()
        {
            ProjectEvent<AccountCreatedEvent>((state, @event) =>
            {
                state.Id = @event.Id;
                state.AccountName = @event.Command.AccountName;
                state.AccountType = @event.Command.AccountType;
                state.UserId = @event.Command.UserId;
                state.IsDefault = @event.Command.IsDefault;
            });

            ProjectEvent<AddedAccountFollowerEvent>((state, @event) =>
            {
                state.FollowersCount += 1;
            });

            ProjectEvent<RemovedAccountFollowerEvent>((state, @event) =>
            {
                state.FollowersCount -= 1;
            });

            ProjectEvent<AccountFollowedEvent>((state, @event) =>
            {
                state.FollowingCount += 1;
            });

            ProjectEvent<AccountUnfollowedEvent>((state, @event) =>
            {
                state.FollowingCount -= 1;
            });

            ProjectEvent<AvatarImageAccountChangedEvent>((state, @event) =>
            {
                state.AvatarUrl = @event.AvatarUrl;
            });

            ProjectEvent<ProfileChangedEvent>((state, @event) =>
            {
                state.Profile = new Profile(
                          name: @event.Name,
                          jobDescription: @event.JobDescription,
                          bioDescription: @event.BioDescription);
            });
        }
    }
}
