using System;
using EzCommon.Models;
using EzGym.Events.Wallet;
using EzPayment.Payments;

namespace EzGym.Wallets;

public class WalletReceipt : AggregateRoot
{
    public string WalletId { get; private set; }
    public string PaymentId { get; private set; }
    public PaymentStatusEnum PaymentStatus { get; private set; }
    public DateTime? PaymentDateTime { get; private set; }
    public long Amount { get; private set; }
    public long ApplicationFeeAmount { get; private set; }
    public string Description { get; private set; }

    public WalletReceipt(string walletId, string paymentId, PaymentStatusEnum paymentStatus, DateTime? paymentDateTime, long amount, long applicationFeeAmount, string description)
    {
        RaiseEvent(new WalletReceiptCreatedEvent(GenerateNewId(), walletId, paymentId, paymentStatus, paymentDateTime, amount, applicationFeeAmount, description));
    }

    public void UpdateReceipt(PaymentStatusEnum paymentStatus, DateTime paymentDateTime)
    {
        RaiseEvent(new WalletReceiptUpdatedEvent(Id, PaymentId, paymentStatus, paymentDateTime));
    }

    protected void Apply(WalletReceiptCreatedEvent @event)
    {
        Id = @event.Id;
        WalletId = @event.WalletId;
        PaymentId = @event.PaymentId;
        PaymentStatus = @event.PaymentStatus;
        PaymentDateTime = null;
        Amount = @event.Amount - @event.ApplicationFeeAmount;
        ApplicationFeeAmount = @event.ApplicationFeeAmount;
        Description = @event.Description;
    }

    protected void Apply(WalletReceiptUpdatedEvent @event)
    {
        PaymentStatus = @event.PaymentStatus;
        PaymentDateTime = @event.PaymentDateTime;
    }

}