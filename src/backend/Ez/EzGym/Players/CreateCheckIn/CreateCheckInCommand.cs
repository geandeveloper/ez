using EzCommon.Commands;

namespace EzGym.Players.CreateCheckIn;

public record CreateCheckInCommand(string PlayerId, string GymAccountId, string MemberShipId) : ICommand;

