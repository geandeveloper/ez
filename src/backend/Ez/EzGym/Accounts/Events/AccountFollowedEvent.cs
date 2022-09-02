using EzCommon.Events;

namespace EzGym.Accounts.Events
{
    public record AccountFollowedEvent(string AccountId, string FollowedAccountId) : Event;
}
