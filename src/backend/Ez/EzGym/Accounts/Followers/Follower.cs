using EzCommon.Models;
using EzGym.Accounts.Events;

namespace EzGym.Accounts.Followers
{
    public class Follower : AggregateRoot
    {
        public string AccountId { get; private set; }
        public string FollowerAccountId { get; private set; }

        public Follower() { }

        public Follower(string accountId, string followerAccountId)
        {
            RaiseEvent(new FollowerCreatedEvent(GenerateNewId(), accountId, followerAccountId));
        }

        public void Unfollow()
        {
            RaiseEvent(new FollowDeleteEvent(Id, AccountId, FollowerAccountId));
        }

        protected void Apply(FollowDeleteEvent @event)
        {
            FollowerAccountId = null;
            AccountId = null;
        }

        protected void Apply(FollowerCreatedEvent @event)
        {
            Id = @event.Id;
            AccountId = @event.AccountId;
            FollowerAccountId = @event.FollowerAccountId;
        }
    }
}
