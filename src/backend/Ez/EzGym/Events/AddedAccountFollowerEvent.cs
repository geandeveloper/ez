using EzCommon.Events;
using System;

namespace EzGym.Events
{
    public record AddedAccountFollowerEvent(Guid AccountId) : Event;

}
