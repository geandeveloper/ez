using EzCommon.Commands;
using System;

namespace EzGym.Features.Gyms.CreateGym
{
    public record CreateGymCommand(Guid UserId) : ICommand;

}
