using EzCommon.Events;
using EzGym.Features.Accounts.CreateAccount;
using System;

namespace EzGym.Events
{
    public record AccountCreatedEvent(Guid Id, CreateAccountCommand Command) : Event;
}
