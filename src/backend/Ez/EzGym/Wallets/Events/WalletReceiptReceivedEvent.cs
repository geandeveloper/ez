using EzCommon.Events;

namespace EzGym.Wallets.Events
{
    public record WalletReceiptReceivedEvent(WalletReceipt Receipt) : Event;
    public record WalletCreatedEvent(string Id, string AccountId) : Event;

}
