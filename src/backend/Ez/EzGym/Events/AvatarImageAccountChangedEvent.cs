using EzCommon.Events;
using System;

namespace EzGym.Events
{
    public record AvatarImageAccountChangedEvent(Guid AccountId, string AvatarUrl) : Event;
}
