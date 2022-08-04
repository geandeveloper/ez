using EzCommon.Events;
using EzGym.Models;

namespace EzGym.Events
{
    public record AddedFollowAccountEvent(Follower Follower) : Event;
}
