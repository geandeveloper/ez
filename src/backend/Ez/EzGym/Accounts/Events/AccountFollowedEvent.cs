using EzCommon.Events;
using System;

namespace EzGym.Accounts.Events
{
    public record AccountFollowedEvent(Guid AccountId) : Event;
}
