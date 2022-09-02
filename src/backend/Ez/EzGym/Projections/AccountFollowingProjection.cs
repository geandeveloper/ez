
using System.Linq;
using System.Threading.Tasks;
using EzGym.Accounts;
using EzGym.Accounts.Events;
using Marten;
using Marten.Events.Aggregation;

namespace EzGym.Projections
{

    public class AccountFollowing
    {
        public string Id { get; set; }
        public string FollowerAccountId { get; set; }
        public string AvatarUrl { get; set; }
        public string AccountName { get; set; }
        public string ProfileName { get; set; }

    }

    public class AccountFollowingsProjection : SingleStreamAggregation<AccountFollowing>
    {
        public AccountFollowingsProjection()
        {
            DeleteEvent<AccountUnfollowedEvent>();

            ProjectEvent<AccountFollowedEvent>((session, state, @event) =>
            {
                var following = session
                    .Query<Account>()
                    .First(a => a.Id == @event.AccountId);

                state.Id = @event.AccountId;
                state.AccountName = following.AccountName;
                state.FollowerAccountId = following.Id;
                state.AvatarUrl = following.AvatarUrl;
                state.AccountName = following.AccountName;
                state.ProfileName = following.Profile?.Name;
            });
        }
    }
}
