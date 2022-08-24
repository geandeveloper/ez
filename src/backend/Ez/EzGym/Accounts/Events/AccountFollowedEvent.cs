using EzCommon.Events;

namespace EzGym.Accounts.Events
{
    public record AccountFollowedEvent(string AccountId) : Event;
}
