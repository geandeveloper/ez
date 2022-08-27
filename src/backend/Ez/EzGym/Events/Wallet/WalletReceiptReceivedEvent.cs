using EzCommon.Events;
using EzGym.Wallets;
using EzGym.Wallets.Projections;

namespace EzGym.Events.Wallet
{
    public record WalletReceiptReceivedEvent(WalletReceipt Receipt) : Event;
    public record WalletCreatedEvent(string Id, string AccountId) : Event;

}
