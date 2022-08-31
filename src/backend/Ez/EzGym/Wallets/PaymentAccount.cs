using EzPayment.PaymentAccounts;

namespace EzGym.Wallets;

public record PaymentAccount(string Id, string OnBoardingLink, PaymentAccountStatusEnum PaymentAccountStatus);