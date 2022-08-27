using EzCommon.Events;
using EzGym.Wallets;

namespace EzGym.Events.Wallet
{
    public record WalletReceiptReceivedEvent(WalletReceipt Receipt) : Event;
    public record WalletCreatedEvent(string Id, string AccountId) : Event;

}
