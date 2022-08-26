using EzGym.Payments;
using System;

namespace EzGym.Wallets;

public record WalletReceipt(string PaymentId, bool Incoming, PaymentMethodEnum PaymentMethod, PaymentStatusEnum PaymentStatus, DateTime? PaymentDateTime, decimal Value, string Description);