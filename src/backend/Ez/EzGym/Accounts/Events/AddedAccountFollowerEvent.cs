using EzCommon.Events;
using System;

namespace EzGym.Accounts.Events
{
    public record AddedAccountFollowerEvent(string AccountId) : Event;

}
