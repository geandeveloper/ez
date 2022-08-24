using System;
using EzCommon.Commands;

namespace EzGym.Accounts.FollowAccount
{
    public record FollowAccountCommand(string UserAccountId, string FollowAccountId) : ICommand;
}
