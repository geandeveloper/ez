using System;
using EzCommon.Commands;

namespace EzGym.Wallets.UpdateWallet
{
    public record UpdateWalletCommand(Guid AccountId, Pix Pix) : ICommand;
}
