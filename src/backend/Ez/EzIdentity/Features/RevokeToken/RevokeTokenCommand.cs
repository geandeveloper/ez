using EzCommon.Commands;

namespace EzIdentity.Features.RevokeToken;
public record RevokeTokenCommand(string RefreshToken) : ICommand;

