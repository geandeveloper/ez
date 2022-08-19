using EzCommon.Commands;
using System;

namespace EzGym.Features.Gyms.CreatePlan
{
    public record CreatePlanCommand(Guid GymId, string Name, int Days, decimal Price, bool Active) : ICommand;
}
