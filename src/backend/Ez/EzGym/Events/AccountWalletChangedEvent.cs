using EzCommon.Events;
using EzGym.Models;
using System;

namespace EzGym.Events
{
    public record AccountWalletChangedEvent(Guid Id, Guid AccountId, decimal Balance, Pix PixInfo) : Event;
}
