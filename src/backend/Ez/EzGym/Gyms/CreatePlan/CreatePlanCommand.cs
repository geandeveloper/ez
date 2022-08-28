using EzCommon.Commands;

namespace EzGym.Gyms.CreatePlan
{
    public record CreatePlanCommand(string GymId, string Name, int Days, long Amount, bool Active) : ICommand;
}
