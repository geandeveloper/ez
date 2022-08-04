using EzCommon.Events;
using EzGym.Models;

namespace EzGym.Events
{
    public record UnfollowAccountEvent(Follower Follower) : Event;
}
