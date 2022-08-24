using EzCommon.Events;

namespace EzGym.Accounts.Events
{
    public record AccountUnfollowedEvent(string AccountId) : Event;
}
