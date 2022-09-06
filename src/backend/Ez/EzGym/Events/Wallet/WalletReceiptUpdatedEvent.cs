using System;
using EzCommon.Events;
using EzPayment.Payments;

namespace EzGym.Events.Wallet;

public record WalletReceiptUpdatedEvent(string Id, string PaymentId, PaymentStatusEnum PaymentStatus, DateTime PaymentDateTime) : Event;