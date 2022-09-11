using EzCommon.Events;

namespace EzGym.Accounts.Events
{
    public record FollowerCreatedEvent(string Id, string AccountId, string FollowerAccountId) : Event;

}
