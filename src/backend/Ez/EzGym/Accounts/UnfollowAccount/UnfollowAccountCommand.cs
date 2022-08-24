using System;
using EzCommon.Commands;

namespace EzGym.Accounts.UnfollowAccount
{
    public record UnfollowAccountCommand(string UserAccountId, string UnfollowAccountId) : ICommand;
}
