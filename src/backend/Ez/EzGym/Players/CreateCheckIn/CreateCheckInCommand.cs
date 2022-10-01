using EzCommon.Commands;

namespace EzGym.Players.CreateCheckIn;

public record CreateCheckInCommand(string PlayerId, string GymId, string MemberShipId) : ICommand;

