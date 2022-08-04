using EzCommon.Events;
using EzGym.Models;
using System;

namespace EzGym.Events
{
    public record StartFollowAccountEvent(Guid AccountId, Follower Follower) : Event;
}
