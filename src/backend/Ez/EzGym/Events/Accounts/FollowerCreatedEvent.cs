using EzCommon.Events;

namespace EzGym.Events.Accounts
{
    public record FollowerCreatedEvent(string Id, string AccountId, string FollowerAccountId) : Event;

}
