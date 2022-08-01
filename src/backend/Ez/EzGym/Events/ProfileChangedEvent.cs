using EzCommon.Events;
using System;

namespace EzGym.Events
{
    public record ProfileChangedEvent(Guid AccountId, string Name, string JobDescription, string BioDescription) : Event;
}
