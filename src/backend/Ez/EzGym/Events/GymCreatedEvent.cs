using EzCommon.Events;
using EzGym.Models;
using System;

namespace EzGym.Events
{
    public record GymCreatedEvent(
         Guid Id,
         Guid OwnerId,
         string FantasyName,
         string Cnpj,
         Address[] Addresses
        ) : Event;
}
