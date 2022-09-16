using EzCommon.Commands;

namespace EzGym.Players.CreatePlayer
{
    public record CreatePlayerCommand(string AccountId) : ICommand;
}
