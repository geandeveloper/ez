using EzCommon.Events;
using System;

namespace EzGym.Events
{
    public record AvatarImageAccountChanged(Guid AccountId, string AvatarUrl) : Event;
}
