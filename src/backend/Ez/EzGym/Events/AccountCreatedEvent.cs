using EzCommon.Events;
using EzGym.Models;
using System;

namespace EzGym.Events
{
    public record AccountCreatedEvent(Guid Id, Guid UserId, string AccountName, AccountTypeEnum AccountType) : Event;
}
