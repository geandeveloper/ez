using System;
using EzPayment.Payments;

namespace EzGym.Wallets;

public record WalletReceipt(string PaymentId, PaymentStatusEnum PaymentStatus, DateTime? PaymentDateTime, long Amount, long ApplicationFeeAmount, string Description);