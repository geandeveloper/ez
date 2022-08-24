using System;
using EzCommon.Commands;

namespace EzGym.Wallets.UpdateWallet
{
    public record UpdateWalletCommand(string AccountId, Pix Pix) : ICommand;
}
