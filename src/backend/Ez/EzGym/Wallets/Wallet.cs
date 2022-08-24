using System.Collections.Generic;
using EzCommon.Models;

namespace EzGym.Wallets
{
    public class Wallet : AggregateRoot
    {
        public string AccountId { get; init; }
        public Pix Pix { get; init; }
        public IEnumerable<WalletReceipt> Receipts { get; init; }

        public Wallet(Pix pix)
        {
            Pix = pix;
        }

    }
}
