using EzCommon.Events;
using System;

namespace EzGym.Accounts.Events
{
    public record AvatarImageAccountChangedEvent(Guid AccountId, string AvatarUrl) : Event;
}
