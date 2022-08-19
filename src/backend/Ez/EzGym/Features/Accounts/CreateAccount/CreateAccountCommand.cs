using EzCommon.Commands;
using System;

namespace EzGym.Features.Accounts.CreateAccount
{
    public record CreateAccountCommand(Guid UserId, string AccountName, bool IsDefault) : ICommand;
}
