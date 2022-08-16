using EzCommon.Models;
using EzGym.Events;
using System;

namespace EzGym.Models
{
    public class Wallet : AggregateRoot
    {
        public Guid AccountId { get; set; }
        public decimal Balance { get; private set; }
        public Pix Pix { get; private set; }

        public Wallet(Guid accountId, decimal balance, Pix pix)
        {
            RaiseEvent(new AccountWalletChangedEvent(Guid.NewGuid(), accountId, balance, pix));
        }

        private void Apply(AccountWalletChangedEvent @event)
        {
        }
    }
}
