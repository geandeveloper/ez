using System;
using EzCommon.Events;
using EzGym.Wallets;
using EzPayment.Payments;

namespace EzGym.Events.Wallet
{
    public record WalletReceiptCreatedEvent(string Id, string WalletId, string PaymentId, PaymentStatusEnum PaymentStatus, DateTime? PaymentDateTime, long Amount, long ApplicationFeeAmount, string Description) : Event;
}
