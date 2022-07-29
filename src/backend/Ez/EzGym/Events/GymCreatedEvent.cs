using EzCommon.Events;
using EzGym.Models;
using System;

namespace EzGym.Events
{
    public record GymCreatedEvent(
         Guid Id,
         Guid AccountId,
         string FantasyName,
         string Cnpj,
         Address[] Addresses
        ) : Event;
}
