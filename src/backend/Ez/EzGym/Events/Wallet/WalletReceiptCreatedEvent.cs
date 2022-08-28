using EzCommon.Events;
using EzGym.Wallets;

namespace EzGym.Events.Wallet
{
    public record WalletReceiptCreatedEvent(WalletReceipt Receipt) : Event;
}
