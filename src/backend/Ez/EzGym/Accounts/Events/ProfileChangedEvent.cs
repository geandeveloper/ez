using EzCommon.Events;
using System;

namespace EzGym.Accounts.Events
{
    public record ProfileChangedEvent(string AccountId, string Name, string JobDescription, string BioDescription) : Event;
}
