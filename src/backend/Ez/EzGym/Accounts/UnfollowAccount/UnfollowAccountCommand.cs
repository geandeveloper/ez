using System;
using EzCommon.Commands;

namespace EzGym.Accounts.UnfollowAccount
{
    public record UnfollowAccountCommand(Guid UserAccountId, Guid UnfollowAccountId) : ICommand;
}
