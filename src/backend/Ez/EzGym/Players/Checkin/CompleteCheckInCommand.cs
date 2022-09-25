
using EzCommon.Commands;

namespace EzGym.Players.CheckIn;

public record CompleteCheckInCommand(string AccountId, string GymId) : ICommand;
