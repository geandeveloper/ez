using EzCommon.Events;
using EzIdentity.Users;

namespace EzIdentity.Events.Users
{
    public record SuccessLoginEvent(AccessToken AccessToken, RefreshToken RefreshToken) : Event;
}
