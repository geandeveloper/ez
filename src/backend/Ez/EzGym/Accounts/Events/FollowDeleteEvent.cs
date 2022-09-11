using EzCommon.Events;

namespace EzGym.Accounts.Events
{
    public record FollowDeleteEvent(string Id, string AccountId, string FollowerAccountId) : Event;
}
