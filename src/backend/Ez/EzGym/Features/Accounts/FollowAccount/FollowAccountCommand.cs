using EzCommon.Commands;
using System;

namespace EzGym.Features.Accounts.FollowAccount
{
    public record FollowAccountCommand(Guid UserAccountId, Guid FollowAccountId) : ICommand;
}
