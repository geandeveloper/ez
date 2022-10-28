using EzCommon.Events;

namespace EzGym.Events.Accounts
{
    public record AvatarImageAccountChangedEvent(string AccountId, string AvatarUrl) : Event;
}
