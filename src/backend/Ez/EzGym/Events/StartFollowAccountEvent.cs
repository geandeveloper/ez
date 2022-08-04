using EzCommon.Events;
using EzGym.Models;

namespace EzGym.Events
{
    public record StartFollowAccountEvent(Follower Follower) : Event;
}
