using EzGym.Models;
using System;

namespace EzGym.Features.Accounts.UpdateWallet
{
    public record UpdateWalletCommand(Guid WalletId, Guid AccountId, decimal Balance, Pix Pix);
}
