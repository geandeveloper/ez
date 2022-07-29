using EzCommon.Commands;
using EzGym.Models;
using System;

namespace EzGym.Features.Accounts.CreateAccount
{
    public record CreateAccountCommand(Guid UserId, string AccountName, AccountTypeEnum AccountType, bool IsDefault) : ICommand;
}
