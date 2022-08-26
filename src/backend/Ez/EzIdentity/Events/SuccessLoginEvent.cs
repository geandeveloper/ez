using EzCommon.Events;
using EzIdentity.Users;

namespace EzIdentity.Events
{
    public record SuccessLoginEvent(AccessToken AccessToken, RefreshToken RefreshToken) : Event;
}
