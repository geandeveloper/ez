using EzCommon.Events;

namespace EzGym.Events.Accounts
{
    public record FollowDeleteEvent(string Id, string AccountId, string FollowerAccountId) : Event;
}
