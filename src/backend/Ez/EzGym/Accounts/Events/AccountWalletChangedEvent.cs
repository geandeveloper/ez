using EzCommon.Events;
using EzGym.Wallets;
using System;

namespace EzGym.Accounts.Events
{
    public record AccountWalletChangedEvent(Guid AccountId, Pix Pix) : Event;
}
