using EzCommon.Events;
using System;
using EzGym.Accounts.CreateAccount;

namespace EzGym.Accounts.Events
{
    public record AccountCreatedEvent(Guid Id, CreateAccountCommand Command) : Event;
}
