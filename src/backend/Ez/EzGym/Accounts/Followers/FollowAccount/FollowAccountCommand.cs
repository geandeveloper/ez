using System;
using EzCommon.Commands;

namespace EzGym.Accounts.Followers.FollowAccount
{
    public record FollowAccountCommand(string UserAccountId, string FollowAccountId) : ICommand;
}
