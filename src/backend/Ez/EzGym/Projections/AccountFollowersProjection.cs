using System.Linq;
using EzGym.Accounts;
using EzGym.Accounts.Events;
using Marten;
using Marten.Events.Aggregation;

namespace EzGym.Projections
{

    public class AccountFollower
    {
        public string Id { get; set; }
        public string FollowerAccountId { get; set; }
        public string AvatarUrl { get; set; }
        public string AccountName { get; set; }
        public string ProfileName { get; set; }

    }

    public class AccountFollowersProjection : SingleStreamAggregation<AccountFollower>
    {
        public AccountFollowersProjection()
        {
            DeleteEvent<RemovedAccountFollowerEvent>();

            ProjectEvent<AddedAccountFollowerEvent>((session, state, @event) =>
            {
                var follower = session
                    .Query<Account>()
                    .First(a => a.Id == @event.FollowerAccountId);

                state.Id = @event.AccountId;
                state.AccountName = follower.AccountName;
                state.FollowerAccountId = follower.Id;
                state.AvatarUrl = follower.AvatarUrl;
                state.AccountName = follower.AccountName;
                state.ProfileName = follower.Profile?.Name;
            });
        }
    }
}
