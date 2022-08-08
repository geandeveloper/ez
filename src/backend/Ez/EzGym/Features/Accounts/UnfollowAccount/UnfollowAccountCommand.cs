using EzCommon.Commands;
using System;

namespace EzGym.Features.Accounts.UnfollowAccount
{
    public record UnfollowAccountCommand(Guid UserAccountId, Guid UnfollowAccountId) : ICommand;
}
