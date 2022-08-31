using EzCommon.Commands;

namespace EzPayment.PaymentAccounts.CreatePaymentAccount;

public record CreatePaymentAccountCommand(string AccountName, string ProfileUrl, string Mcc) : ICommand;