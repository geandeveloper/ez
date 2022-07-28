using EzCommon.Commands;
using EzGym.Features.Dtos;
using System;

namespace EzGym.Features.Gyms.CreateGym
{
    public record CreateGymCommand(
        Guid OwnerId,
        string FantasyName,
        string Cnpj, AddressDto[] Addresses) : ICommand;

}
