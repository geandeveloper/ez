using EzCommon.Events;
using EzIdentity.Models;

namespace EzIdentity.Events
{
    public record SucessLoginEvent(string AccessToken) : Event;
}
