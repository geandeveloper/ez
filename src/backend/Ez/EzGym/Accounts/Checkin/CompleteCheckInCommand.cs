using EzCommon.Commands;

namespace EzGym.Accounts.CheckIn;

public record CompleteCheckInCommand(string AccountId, string GymId) : ICommand;