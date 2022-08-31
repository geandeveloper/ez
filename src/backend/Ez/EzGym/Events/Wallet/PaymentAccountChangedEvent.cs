using EzCommon.Events;
using EzPayment.PaymentAccounts;

namespace EzGym.Events.Wallet;

public record PaymentAccountChangedEvent(string WalletId, string PaymentAccountId, string OnBoardingLink, PaymentAccountStatusEnum PaymentAccountStatus) : Event;