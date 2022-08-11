using EzCommon.Events;
using EzGym.Models;
using System;

namespace EzGym.Events
{
    public record GymCreatedEvent(
         Guid Id,
         Guid AccountId,
         Address[] Addresses
        ) : Event;
}
