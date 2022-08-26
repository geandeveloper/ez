using EzCommon.Commands;

namespace EzIdentity.Users.RevokeToken;
public record RevokeTokenCommand(string RefreshToken) : ICommand;

