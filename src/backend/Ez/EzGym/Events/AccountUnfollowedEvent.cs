using EzCommon.Events;
using System;

namespace EzGym.Events
{
    public record AccountUnfollowedEvent(Guid AccountId) : Event;
}
