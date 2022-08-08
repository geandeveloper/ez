using EzCommon.Events;
using System;

namespace EzGym.Events
{
    public record RemovedAccountFollowerEvent(Guid AccountId) : Event;

}
