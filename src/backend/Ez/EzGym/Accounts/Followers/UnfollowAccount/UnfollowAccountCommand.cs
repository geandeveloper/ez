using System;
using EzCommon.Commands;

namespace EzGym.Accounts.Followers.UnfollowAccount
{
    public record UnfollowAccountCommand(string UserAccountId, string UnfollowAccountId) : ICommand;
}
