using EzCommon.Events;
using EzGym.Accounts.CreateAccount;

namespace EzGym.Accounts.Events
{
    public record AccountCreatedEvent(string Id, CreateAccountCommand Command) : Event;
}
