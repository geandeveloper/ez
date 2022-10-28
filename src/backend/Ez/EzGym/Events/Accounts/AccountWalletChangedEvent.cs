using EzCommon.Events;
using EzGym.Wallets;
using System;

namespace EzGym.Events.Accounts
{
    public record AccountWalletChangedEvent(string AccountId, Pix Pix) : Event;
}
