using System;
using System.Collections.Generic;
using System.Linq;
using EzCommon.Models;
using EzGym.Events.Wallet;
using EzPayment.Payments;

namespace EzGym.Wallets
{
    public class Wallet : AggregateRoot
    {
        public string AccountId { get; private set; }
        public Pix Pix { get; private set; }
        public IList<WalletReceipt> Receipts { get; } = new List<WalletReceipt>();

        private Wallet() { }

        public Wallet(string accountId)
        {
            RaiseEvent(new WalletCreatedEvent(GenerateNewId(), accountId));
        }

        public void UpdateWallet(Pix pix)
        {
            RaiseEvent(new WalletUpdatedEvent(Id, pix));
        }

        public void AddReceipt(string paymentId, PaymentStatusEnum paymentStatus, DateTime? paymentDateTime, decimal value, string description)
        {
            RaiseEvent(new WalletReceiptCreatedEvent(new WalletReceipt(paymentId, paymentStatus, paymentDateTime, value, description)));
        }

        public void UpdateReceipt(string paymentId, Func<WalletReceipt, WalletReceipt> updateReceipt)
        {

            Receipts.Where(r => r.PaymentId == paymentId).ToList().ForEach(r =>
            {
                RaiseEvent(new WalletReceiptCreatedEvent(r));
            });
        }

        protected void Apply(WalletReceiptUpdatedEvent @event)
        {
            var receipt = Receipts.First(r => r.PaymentId == @event.Receipt.PaymentId);
            Receipts.Remove(receipt);
            Receipts.Add(@event.Receipt);
        }

        protected void Apply(WalletCreatedEvent @event)
        {
            Id = @event.Id;
            AccountId = @event.AccountId;
        }

        protected void Apply(WalletUpdatedEvent @event)
        {
            Pix = @event.Pix;
        }

        protected void Apply(WalletReceiptCreatedEvent @event)
        {
            Receipts.Add(@event.Receipt);
        }

    }
}
