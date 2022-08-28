using EzCommon.Events;
using EzGym.Wallets;

namespace EzGym.Events.Wallet;

public record WalletReceiptUpdatedEvent(WalletReceipt Receipt) : Event;