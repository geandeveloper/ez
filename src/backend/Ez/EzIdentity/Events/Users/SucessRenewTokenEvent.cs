using EzCommon.Events;
using EzIdentity.Users;

namespace EzIdentity.Events.Users
{
    public record SucessRenewTokenEvent(AccessToken AccessToken, RefreshToken RefreshToken) : Event;
}
