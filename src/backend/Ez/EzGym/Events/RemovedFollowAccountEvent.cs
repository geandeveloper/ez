using EzCommon.Events;
using EzGym.Models;

namespace EzGym.Events
{
    public record RemovedFollowAccountEvent(Follower Follower) : Event;
}
