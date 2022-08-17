using EzCommon.Commands;
using EzGym.Models;
using System;

namespace EzGym.Features.Accounts.UpdateWallet
{
    public record UpdateWalletCommand(Guid AccountId, Pix Pix) : ICommand;
}
