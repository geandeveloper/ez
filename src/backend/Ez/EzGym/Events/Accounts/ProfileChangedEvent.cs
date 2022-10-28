using EzCommon.Events;
using System;

namespace EzGym.Events.Accounts
{
    public record ProfileChangedEvent(string AccountId, string Name, string JobDescription, string BioDescription) : Event;
}
