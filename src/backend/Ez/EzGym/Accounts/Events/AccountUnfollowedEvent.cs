using EzCommon.Events;
using System;

namespace EzGym.Accounts.Events
{
    public record AccountUnfollowedEvent(Guid AccountId) : Event;
}
