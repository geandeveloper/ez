using EzCommon.Events;
using System;

namespace EzGym.Accounts.Events
{
    public record ProfileChangedEvent(Guid AccountId, string Name, string JobDescription, string BioDescription) : Event;
}
