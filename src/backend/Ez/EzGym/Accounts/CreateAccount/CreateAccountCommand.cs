using System;
using EzCommon.Commands;

namespace EzGym.Accounts.CreateAccount
{
    public record CreateAccountCommand(Guid UserId, string AccountName, AccountTypeEnum AccountType, bool IsDefault) : ICommand;
}
