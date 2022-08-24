using EzCommon.Commands;

namespace EzGym.Gyms.CreatePlan
{
    public record CreatePlanCommand(string GymId, string Name, int Days, decimal Price, bool Active) : ICommand;
}
