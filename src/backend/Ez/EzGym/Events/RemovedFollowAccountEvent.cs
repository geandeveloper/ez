using EzCommon.Events;
using EzGym.Models;
using System;

namespace EzGym.Events
{
    public record RemovedFollowAccountEvent(Guid AccountId, Follower Folower) : Event;
}
