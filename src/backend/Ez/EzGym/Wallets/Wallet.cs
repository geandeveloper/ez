using System;
using System.Collections.Generic;
using System.Linq;
using EzCommon.Models;
using EzGym.Payments;

namespace EzGym.Wallets
{
    public class Wallet : AggregateRoot
    {
        public Guid AccountId { get; init; }
        public Pix Pix { get; init; }
        public IEnumerable<PaymentReceipt> Incoming { get; init; }
        public IEnumerable<PaymentReceipt> Outgoing { get; init; }
        public decimal Balance => Incoming.Where(i => i.Status == PaymentStatusEnum.Approved).Sum(i => i.Value);
        public decimal PendingBalance => Incoming.Where(i => i.Status == PaymentStatusEnum.Pending).Sum(i => i.Value);

        public Wallet(Pix pix)
        {
            Pix = pix;
            Incoming = new List<PaymentReceipt>();
            Outgoing = new List<PaymentReceipt>();
        }

    }
}
