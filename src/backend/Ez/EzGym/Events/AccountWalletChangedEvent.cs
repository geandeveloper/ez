using EzCommon.Events;
using EzGym.Models;
using System;

namespace EzGym.Events
{
    public record AccountWalletChangedEvent(Guid AccountId, Pix Pix) : Event;
}
