using EzCommon.Events;
using EzIdentity.Models;

namespace EzIdentity.Events
{
    public record SucessLoginEvent(AccessToken AccessToken, RefreshToken RefreshToken) : Event;
}
