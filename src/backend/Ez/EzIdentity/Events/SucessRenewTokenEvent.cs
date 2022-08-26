using EzCommon.Events;
using EzIdentity.Users;

namespace EzIdentity.Events
{
    public record SucessRenewTokenEvent(AccessToken AccessToken, RefreshToken RefreshToken) : Event;
}
