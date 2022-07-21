using EzCommon.Events;
using System;

namespace EzIdentity.Events
{
    public record UserCreatedEvent(
         Guid Id,
         string Name,
         string Email,
         string Password
        ) : Event;
}
