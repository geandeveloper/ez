using EzCommon.Events;
using System;

namespace EzGym.Events
{
    public record AccountFollowedEvent(Guid AccountId) : Event;
}
