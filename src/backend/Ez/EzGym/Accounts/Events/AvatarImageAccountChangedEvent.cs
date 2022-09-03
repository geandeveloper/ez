using EzCommon.Events;

namespace EzGym.Accounts.Events
{
    public record AvatarImageAccountChangedEvent(string AccountId, string AvatarUrl) : Event;
}
