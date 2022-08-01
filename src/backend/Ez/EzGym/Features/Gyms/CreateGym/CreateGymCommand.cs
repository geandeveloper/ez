using EzCommon.Commands;
using EzGym.Dtos;
using System;

namespace EzGym.Features.Gyms.CreateGym
{
    public record CreateGymCommand(
        Guid AccountId,
        string FantasyName,
        string Cnpj, AddressDto[] Addresses) : ICommand;

}
