using System;
using EzPayment.Payments;

namespace EzGym.Wallets;

public record WalletReceipt(string PaymentId, PaymentStatusEnum PaymentStatus, DateTime? PaymentDateTime, decimal Value, string Description);