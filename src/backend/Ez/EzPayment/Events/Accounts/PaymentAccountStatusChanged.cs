using EzCommon.Events;
using EzPayment.PaymentAccounts;

namespace EzPayment.Events.Accounts;

public record PaymentAccountStatusChanged(string PaymentAccountId, PaymentAccountStatusEnum Status) : Event;