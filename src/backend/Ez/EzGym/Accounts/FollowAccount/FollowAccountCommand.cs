using System;
using EzCommon.Commands;

namespace EzGym.Accounts.FollowAccount
{
    public record FollowAccountCommand(Guid UserAccountId, Guid FollowAccountId) : ICommand;
}
