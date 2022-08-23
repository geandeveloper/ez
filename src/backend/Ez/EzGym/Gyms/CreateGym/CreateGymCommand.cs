using System;
using EzCommon.Commands;

namespace EzGym.Gyms.CreateGym
{
    public record CreateGymCommand(Guid AccountId) : ICommand;

}
