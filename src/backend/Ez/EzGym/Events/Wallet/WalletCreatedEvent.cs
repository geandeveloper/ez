using EzCommon.Events;

namespace EzGym.Events.Wallet
{
    public record WalletCreatedEvent(string Id, string AccountId) : Event;
}
