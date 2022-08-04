using EzCommon.Events;
using EzGym.Models;
using System;

namespace EzGym.Events
{
    public record UnfollowAccountEvent(Guid AccountId, Follower Follower) : Event;
}
