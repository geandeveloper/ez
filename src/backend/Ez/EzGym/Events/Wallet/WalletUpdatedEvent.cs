using EzCommon.Events;
using EzGym.Wallets;

namespace EzGym.Events.Wallet;

public record WalletUpdatedEvent(string WalletId, Pix Pix) : Event;