using EzCommon.Events;
using System;

namespace EzGym.Accounts.Events
{
    public record RemovedAccountFollowerEvent(string AccountId) : Event;

}
