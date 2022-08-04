using EzCommon.Events;
using EzGym.Models;
using System;

namespace EzGym.Events
{
    public record AddedFollowAccountEvent(Guid AccountId, Follower Follower) : Event;
}
