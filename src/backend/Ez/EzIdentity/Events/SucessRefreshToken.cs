using EzCommon.Events;
using EzIdentity.Models;

namespace EzIdentity.Events
{
    public record SucessRenewTokenEvent(AccessToken AccessToken, RefreshToken RefreshToken) : Event;
    public record SucessRevokeTokenEvent() : Event;
}
