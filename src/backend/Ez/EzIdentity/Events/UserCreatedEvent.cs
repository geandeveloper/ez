using EzCommon.Events;

namespace EzIdentity.Events
{
    public record UserCreatedEvent(
         string Id,
         string Name,
         string UserName,
         string Email,
         string Password
        ) : Event;
}
