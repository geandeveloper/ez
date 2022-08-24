
using EzCommon.Commands;

namespace EzGym.Gyms.CreateGym
{
    public record CreateGymCommand(string AccountId) : ICommand;

}
