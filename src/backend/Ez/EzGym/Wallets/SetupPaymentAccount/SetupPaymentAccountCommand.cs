using EzCommon.Commands;

namespace EzGym.Wallets.SetupPaymentAccount;

public record SetupPaymentAccountCommand(string WalletId, string RefreshUrl, string ReturnUrl) : ICommand;