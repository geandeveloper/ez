using System;

namespace EzGym.Models
{
    public record Wallet
    {
        public Guid AccountId { get; init; }
        public decimal Balance { get; init; }
        public Pix Pix { get; init; }

        public Wallet(Guid accountId, Pix pix)
        {
            AccountId = accountId;
            Balance = 0;
            Pix = pix;
        }
    }
}
