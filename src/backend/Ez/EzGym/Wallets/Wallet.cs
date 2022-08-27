using System;
using System.Collections.Generic;
using EzCommon.Models;
using EzGym.Events.Wallet;
using EzPayment.Payments;

namespace EzGym.Wallets
{
    public class Wallet : AggregateRoot
    {
        public string AccountId { get; private set; }
        public Pix Pix { get; private set; }
        public IList<WalletReceipt> Receipts { get; private set; } = new List<WalletReceipt>();

        private Wallet() { }

        public Wallet(string accountId)
        {
            RaiseEvent(new WalletCreatedEvent(GenerateNewId(), accountId));
        }

        public void AddReceipt(string paymentId, bool incoming, PaymentMethodEnum paymentMethod, PaymentStatusEnum paymentStatus, DateTime? paymentDateTime, decimal value, string description)
        {
            RaiseEvent(new WalletReceiptReceivedEvent(new WalletReceipt(paymentId, incoming, paymentMethod, paymentStatus, paymentDateTime, value, description)));
        }
        protected void Apply(WalletCreatedEvent @event)
        {
            Id = @event.Id;
            AccountId = @event.AccountId;
        }

        protected void Apply(WalletReceiptReceivedEvent @event)
        {
            Receipts.Add(@event.Receipt);
        }

    }
}
