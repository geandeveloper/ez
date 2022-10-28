using EzCommon.Events;
using EzGym.Accounts.CreateAccount;

namespace EzGym.Events.Accounts
{
    public record AccountCreatedEvent(string Id, CreateAccountCommand Command) : Event;
}
