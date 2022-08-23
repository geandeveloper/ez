using System;
using EzCommon.Commands;

namespace EzGym.Gyms.CreatePlan
{
    public record CreatePlanCommand(Guid GymId, string Name, int Days, decimal Price, bool Active) : ICommand;
}
